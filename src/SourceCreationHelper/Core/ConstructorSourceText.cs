using System.Collections.Generic;
using System.Text;
using SourceCreationHelper.Interfaces;

namespace SourceCreationHelper.Core
{
    internal class ConstructorSourceText : SourceTextBase, IConstructorSourceText
    {
        private readonly AccessModifier _accessModifier;
        private readonly List<string> _parameters;
        private readonly List<ISourceText> _members;

        public ConstructorSourceText(AccessModifier accessModifier, string name)
        {
            _accessModifier = accessModifier;
            Name = name;
            IsStatic = false;
            _parameters = new List<string>();
            _members = new List<ISourceText>();
        }

        protected override string BuildSource()
        {
            var sourceStringBuilder = new StringBuilder();
            sourceStringBuilder.AppendLine($"{_accessModifier} {(IsStatic ? "static" : "")} {Name}");
            sourceStringBuilder.Append("(");
            if (_parameters.NotNullAny())
            {
                sourceStringBuilder.Append(string.Join(", ", _parameters));
            }

            sourceStringBuilder.Append(")");
            sourceStringBuilder.AppendLine("{");
            foreach (var memberItem in _members)
            {
                sourceStringBuilder.Append(memberItem.Text);
            }

            sourceStringBuilder.AppendLine("}");
            return sourceStringBuilder.ToString();
        }

        public IConstructorSourceText SetAsStatic(bool isStatic = true)
        {
            IsStatic = isStatic;
            return this;
        }

        public IConstructorSourceText AddParameter(string type, string name)
        {
            _parameters.Add($"{type} {name}");
            return this;
        }

        public IConstructorSourceText AddSource(ISourceText source)
        {
            _members.Add(source);
            return this;
        }

        public IConstructorSourceText AddSource(string source)
        {
            _members.Add(new SourceText(source));
            return this;
        }
        
        public bool IsStatic { get; private set; }
        public string Name { get; }

        public IReadOnlyCollection<string> Parameters => _parameters;
    }
}