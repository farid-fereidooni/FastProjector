using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SourceCreationHelper.Interfaces;

namespace SourceCreationHelper.Core
{
    internal class ClassSourceText: SourceTextBase, IClassSourceText
    {
        private readonly AccessModifier _accessModifier;
        private bool _isStatic;
        private bool _isVirtual;
        private readonly List<ISourceText> _members;
        private readonly List<IConstructorSourceText> _constructors;

        public ClassSourceText(string name, AccessModifier accessModifier)
        {
            Name = name;
            _accessModifier = accessModifier;
            _isStatic = false;
            _isVirtual = false;
            _members = new List<ISourceText>();
            _constructors = new List<IConstructorSourceText>();

        }
        protected override string BuildSource()
        {
            var sourceStringBuilder = new StringBuilder();
            var virtualExpression = _isVirtual ? "virtual" : "";
            sourceStringBuilder.AppendLine($"{_accessModifier} {(_isStatic? "static": virtualExpression)} class {Name}");
            sourceStringBuilder.AppendLine("{");
            foreach(var memberItem in _members)
            {
                sourceStringBuilder.Append(memberItem.Text);
            }
            sourceStringBuilder.AppendLine("}");
            return sourceStringBuilder.ToString();
        }

        public IClassSourceText AddConstructor(IConstructorSourceText constructorSource)
        {
            if (!constructorSource.Name.Equals(Name))
                throw new ArgumentException("Constructor should be same name as the class name",
                    nameof(constructorSource));
            
            if (_constructors.Any(ctor =>
                    ctor.IsStatic == constructorSource.IsStatic && ctor.Parameters.SequenceEqual(constructorSource.Parameters)))
                throw new InvalidOperationException("A constructor with exactly same signature has already been added");
            _constructors.Add(constructorSource);

            return this;
        }

        public IClassSourceText SetAsStatic(bool isStatic = true)
        {
            _isStatic = isStatic;
            return this;
        }

        public IClassSourceText SetAsVirtual(bool isVirtual = true)
        {
            _isVirtual = isVirtual;
            return this;
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