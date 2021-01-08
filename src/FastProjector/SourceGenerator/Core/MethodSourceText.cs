using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastProjector.MapGenerator.SourceGenerator.Interfaces
{   
    internal class MethodSourceText: IMethodSourceText
    {
        private readonly AccessModifier accessModifier;
        private readonly string returnType;
        private readonly string name;
        private readonly IEnumerable<string> parameters;
        private readonly bool isStatic;
        private readonly bool isVirtual;
        private readonly bool isAsync;
        private readonly List<ISourceText> members;

        public string Text => BuildSource();

        public MethodSourceText(AccessModifier accessModifier, string returnType, string name, IEnumerable<string> parameters, bool isStatic = false, bool isVirtual = false, bool isAsync = false)
        {
            this.accessModifier = accessModifier;
            this.returnType = returnType;
            this.name = name;
            this.parameters = parameters;
            this.isStatic = isStatic;
            this.isVirtual = isVirtual;
            this.isAsync = isAsync;
            members = new List<ISourceText>();
        }

        public string BuildSource()
        {
             var sourceStringBuilder = new StringBuilder();
            sourceStringBuilder.AppendLine($"{accessModifier} {(isAsync? "async " : "" )}{(isStatic? "static": (isVirtual? "virtual" : ""))} {returnType} {name}");
            sourceStringBuilder.Append("(");
            if(parameters.NotNullAny())
            {
                sourceStringBuilder.Append(string.Join(", ", parameters));
            }
            sourceStringBuilder.Append(")");
            sourceStringBuilder.AppendLine("{");
            foreach(var memberItem in members)
            {
                sourceStringBuilder.Append(memberItem.Text);
            }
            sourceStringBuilder.AppendLine("}");
            return sourceStringBuilder.ToString();
        }

        public IMethodSourceText AddSource(ISourceText source)
        {
            members.Add(source);
            return this;
        }

        public IMethodSourceText AddSource(string source)
        {
            members.Add(new SourceText(source));
            return this;
        }
    }
}