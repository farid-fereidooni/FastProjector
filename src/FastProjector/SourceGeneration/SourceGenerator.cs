using System.Collections.Generic;
using FastProjector.MapGenerator.SourceGeneration.Core;
using FastProjector.MapGenerator.SourceGeneration.Interfaces;
using FastProjector.SourceGeneration.Core;

namespace FastProjector.MapGenerator.SourceGeneration
{
    internal static class SourceGenerator
    {
        public static INamespaceSourceText CreateNamespace(string name)
        {
            return new NamespaceSourceText(name);
        }

        public static IClassSourceText CreateClass(string name, AccessModifier accessModifier, bool isStatic = false, bool isVirtual = false)
        {
            return new ClassSourceText(name, accessModifier, isStatic, isVirtual);
        }

        public static IMethodSourceText CreateMethod(AccessModifier accessModifier, string returnType, string name, IEnumerable<string> parameters, bool isStatic = false, bool isVirtual = false, bool isAsync = false)
        {
            return new MethodSourceText(accessModifier, returnType, name, parameters, isStatic, isVirtual, isAsync);
        }

        public static IPropertySourceText CreateProperty(AccessModifier accessModifier, string type, string name, string initializer = null)
        {
            return new PropertySourceText(accessModifier, type, name, initializer);
        }

        public static IFieldSourceText CreateField(AccessModifier accessModifier, string type, string name, string initializer = null)
        {
            return new FieldSourceText(accessModifier, type, name, initializer);
        }

        public static IBlockSourceText CreateCodeBlock(string blockExpression)
        {
            return new BlockSourceText(blockExpression);
        }

        public static IAssignmentSourceText CreateAssignment(ISourceText left, ISourceText right)
        {
            return new AssignmentSourceText(left, right);
        }

        public static IMemberInitSourceText CreateMemberInit(params IAssignmentSourceText[] assignments)
        {
            return new MemberInitSourceText(assignments);
        }
        
        public static IMemberInitSourceText CreateMemberInit(IEnumerable<IAssignmentSourceText> assignments)
        {
            return new MemberInitSourceText(assignments);
        }

        public static ICallSourceText CreateCall(string methodName)
        {
            return new CallSourceText(methodName);
        }
        
        public static ILambdaExpressionSourceText CreateLambdaExpression()
        {
            return new LambdaExpressionSourceText();
        }
        
        public static ISourceText CreateSource(string source)
        {
            return new SourceText(source);
        }

    }
}