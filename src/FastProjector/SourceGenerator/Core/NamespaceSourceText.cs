using System.Collections.Generic;
using System.Text;
using FastProjector.MapGenerator.SourceGenerator.Interfaces;

namespace FastProjector.MapGenerator.SourceGenerator.Interfaces
{   
    internal class NamespaceSourceText: ISourceText
    {
        private List<IClassSourceText> classes;
        private readonly string namespaceName;
        public NamespaceSourceText(string name)
        {
            namespaceName = name;
            classes = new List<IClassSourceText>();
        }

        public string Text => BuildSource();

        public void AddClass(IClassSourceText classSource)
        {
            classes.Add(classSource);
        }

        private string BuildSource()
        {
            var sourceStringBuilder = new StringBuilder();
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