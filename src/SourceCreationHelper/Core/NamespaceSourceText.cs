using System.Collections.Generic;
using System.Text;
using SourceCreationHelper.Interfaces;

namespace SourceCreationHelper.Core
{   
    internal class NamespaceSourceText: SourceTextBase, INamespaceSourceText
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

        public INamespaceSourceText AddClass(IClassSourceText classSource)
        {
            classes.Add(classSource);
            return this;
        }
        public INamespaceSourceText AddUsing(string packageName)
        {
            usings.Add(packageName);
            return this;
        }

        protected override string BuildSource()
        {
            var sourceStringBuilder = new StringBuilder();
            foreach(var usingExpression in usings)
            {
                sourceStringBuilder.Append("using ");
                sourceStringBuilder.Append(usingExpression);
                sourceStringBuilder.AppendLine(";");
            }
            sourceStringBuilder.AppendLine($"namespace {namespaceName}");
            sourceStringBuilder.AppendLine("{");
            foreach(var classItem in classes)
            {
                sourceStringBuilder.AppendLine(classItem.Text);
            }
            sourceStringBuilder.AppendLine("}");
            return sourceStringBuilder.ToString();
        }

    }
}