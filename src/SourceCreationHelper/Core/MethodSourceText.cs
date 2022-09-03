using System.Collections.Generic;
using System.Text;
using SourceCreationHelper.Interfaces;

namespace SourceCreationHelper.Core
{   
    internal class MethodSourceText: SourceTextBase, IMethodSourceText
    {
        private readonly AccessModifier _accessModifier;
        private readonly string _returnType;
        private readonly List<string> _parameters;
        private readonly List<ISourceText> _members;
        private bool _isStatic;
        private bool _isVirtual;
        private bool _isAsync;
        public string Name { get; }

        public MethodSourceText(AccessModifier accessModifier, string returnType, string name)
        {
            _accessModifier = accessModifier;
            _returnType = returnType;
            Name = name;
            _isStatic = false;
            _isVirtual = false;
            _isAsync = false;
            _parameters = new List<string>();
            _members = new List<ISourceText>();
        }

        protected override string BuildSource()
        {
             var sourceStringBuilder = new StringBuilder();
            sourceStringBuilder.Append($"{_accessModifier} {(_isAsync? "async " : "" )}{(_isStatic? "static": (_isVirtual? "virtual" : ""))} {_returnType} {Name}");
            sourceStringBuilder.Append("(");
            if(_parameters.NotNullAny())
            {
                sourceStringBuilder.Append(string.Join(", ", _parameters));
            }
            sourceStringBuilder.AppendLine(")");
            sourceStringBuilder.AppendLine("{");
            foreach(var memberItem in _members)
            {
                sourceStringBuilder.AppendLine(memberItem.Text);
            }
            sourceStringBuilder.AppendLine("}");
            return sourceStringBuilder.ToString();
        }

        public IMethodSourceText SetAsStatic(bool isStatic = true)
        {
            _isStatic = isStatic;
            return this;
        }
        
        public IMethodSourceText SetAsVirtual(bool isVirtual = true)
        {
            _isVirtual = isVirtual;
            return this;
        }
        
        public IMethodSourceText SetAsAsync(bool isAsync = true)
        {
            _isAsync = isAsync;
            return this;
        }

        
        public IMethodSourceText AddParameter(string type, string name)
        {
            _parameters.Add($"{type} {name}");
            return this;
        }

        public IMethodSourceText AddSource(ISourceText source)
        {
            _members.Add(source);
            return this;
        }

        public IMethodSourceText AddSource(string source)
        {
            _members.Add(new SourceText(source));
            return this;
        }
    }
}