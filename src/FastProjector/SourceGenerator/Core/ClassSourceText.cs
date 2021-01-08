using System.Collections.Generic;
using System.Linq;
using System.Text;
using FastProjector.MapGenerator.SourceGenerator.Interfaces;

namespace FastProjector.MapGenerator.SourceGenerator.Core
{
    internal class ClassSourceText: IClassSourceText
    {
        private readonly string name;
        private readonly AccessModifier accessModifier;
        private readonly bool isStatic;
        private readonly bool isVirtual;
        private List<ISourceText> members;

        public string Text => BuildSource();
        
        public ClassSourceText(string name, AccessModifier accessModifier, bool isStatic = false, bool isVirtual = false)
        {
            this.name = name;
            this.accessModifier = accessModifier;
            this.isStatic = isStatic;
            this.isVirtual = isVirtual;
            members = new List<ISourceText>();

        }
        private string BuildSource()
        {
            var sourceStringBuilder = new StringBuilder();
            sourceStringBuilder.AppendLine($"{accessModifier} {(isStatic? "static": (isVirtual? "virtual" : ""))} {name}");
            sourceStringBuilder.AppendLine("{");
            foreach(var memberItem in members)
            {
                sourceStringBuilder.Append(memberItem.Text);
            }
            sourceStringBuilder.AppendLine("}");
            return sourceStringBuilder.ToString();
        }

        public IClassSourceText AddField(IFieldSourceText fieldSource)
        {
            members.Add(fieldSource);
            return this;
        }

        public IClassSourceText AddProperty(IPropertySourceText propertySource)
        {
            members.Add(propertySource);
            return this;
        }

        public IClassSourceText AddMethod(IMethodSourceText methodSource)
        {
            members.Add(methodSource);
            return this;
        }
    }
}