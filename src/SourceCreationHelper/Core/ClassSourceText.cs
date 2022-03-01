using System.Collections.Generic;
using System.Text;
using SourceCreationHelper.Interfaces;

namespace SourceCreationHelper.Core
{
    internal class ClassSourceText: SourceTextBase, IClassSourceText
    {
        private readonly AccessModifier _accessModifier;
        private readonly bool _isStatic;
        private readonly bool _isVirtual;
        private readonly List<ISourceText> _members;

        public ClassSourceText(string name, AccessModifier accessModifier, bool isStatic = false, bool isVirtual = false)
        {
            Name = name;
            _accessModifier = accessModifier;
            _isStatic = isStatic;
            _isVirtual = isVirtual;
            _members = new List<ISourceText>();

        }
        protected override string BuildSource()
        {
            var sourceStringBuilder = new StringBuilder();
            sourceStringBuilder.AppendLine($"{_accessModifier} {(_isStatic? "static": (_isVirtual? "virtual" : ""))} class {Name}");
            sourceStringBuilder.AppendLine("{");
            foreach(var memberItem in _members)
            {
                sourceStringBuilder.Append(memberItem.Text);
            }
            sourceStringBuilder.AppendLine("}");
            return sourceStringBuilder.ToString();
        }

        public IClassSourceText AddField(IFieldSourceText fieldSource)
        {
            _members.Add(fieldSource);
            return this;
        }

        public IClassSourceText AddProperty(IPropertySourceText propertySource)
        {
            _members.Add(propertySource);
            return this;
        }

        public IClassSourceText AddMethod(IMethodSourceText methodSource)
        {
            _members.Add(methodSource);
            return this;
        }

        public string Name { get; }
    }
}