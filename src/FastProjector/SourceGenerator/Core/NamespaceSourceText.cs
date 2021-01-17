using System.Collections.Generic;
using System.Text;
using FastProjector.MapGenerator.SourceGenerator.Interfaces;

namespace FastProjector.MapGenerator.SourceGenerator.Core
{   
    internal class NamespaceSourceText: INamespaceSourceText
    {
        private readonly List<IClassSourceText> classes;
        private readonly string namespaceName;
        private readonly List<string> usings;
        public NamespaceSourceText(string name)
        {
            namespaceName = name;
            classes = new List<IClassSourceText>();
            usings = new List<string>();
        }

        public string Text => BuildSource();

        public INamespaceSourceText AddClass(IClassSourceText classSource)
        {
            classes.Add(classSource);
            return this;
        }
        public INamespaceSourceText AddUsing(string usingExpression)
        {
            usings.Add(usingExpression);
            return this;
        }

        private string BuildSource()
        {
            var sourceStringBuilder = new StringBuilder();
            foreach(var usingExpression in usings)
            {
                sourceStringBuilder.AppendLine("using ");
                sourceStringBuilder.Append(usingExpression);
                sourceStringBuilder.Append(";");
            }
            sourceStringBuilder.AppendLine($"namespace {namespaceName}");
            sourceStringBuilder.AppendLine("{");
            foreach(var classItem in classes)
            {
                sourceStringBuilder.Append(classItem.Text);
            }
            sourceStringBuilder.AppendLine("}");
            return sourceStringBuilder.ToString();
        }

    }
}