using System.Collections.Generic;
using FastProjector.MapGenerator.SourceGenerator.Core;
using FastProjector.MapGenerator.SourceGenerator.Interfaces;

namespace FastProjector.MapGenerator.SourceGenerator
{
    internal class SourceGenerator
    {
        public INamespaceSourceText CreateNamespace(string name)
        {
            return new NamespaceSourceText(name);
        }

        public IClassSourceText CreateClass(string name, AccessModifier accessModifier, bool isStatic = false, bool isVirtual = false)
        {
            return new ClassSourceText(name, accessModifier, isStatic, isVirtual);
        }

        public IMethodSourceText CreateMethod(AccessModifier accessModifier, string returnType, string name, IEnumerable<string> parameters, bool isStatic = false, bool isVirtual = false, bool isAsync = false)
        {
            return new MethodSourceText(accessModifier, returnType, name, parameters, isStatic, isVirtual, isAsync);
        }

        public IPropertySourceText CreateProperty(AccessModifier accessModifier, string type, string name, string initializer = null)
        {
            return new PropertySourceText(accessModifier, type, name, initializer);
        }

        public IFieldSourceText CreateField(AccessModifier accessModifier, string type, string name, string initializer = null)
        {
            return new FieldSourceText(accessModifier, type, name, initializer);
        }

        public IBlockSourceText CreateCodeBlock(string blockExpression)
        {
            return new BlockSourceText(blockExpression);
        }

    }
}