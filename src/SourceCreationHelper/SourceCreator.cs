using System.Collections.Generic;
using SourceCreationHelper.Core;
using SourceCreationHelper.Interfaces;

namespace SourceCreationHelper
{
    public static class SourceCreator
    {
        public static INamespaceSourceText CreateNamespace(string name)
        {
            return new NamespaceSourceText(name);
        }

        public static IClassSourceText CreateClass(string name, AccessModifier accessModifier)
        {
            return new ClassSourceText(name, accessModifier);
        }

        public static IMethodSourceText CreateMethod(AccessModifier accessModifier, string returnType, string name)
        {
            return new MethodSourceText(accessModifier, returnType, name);
        }

        public static IConstructorSourceText CreateConstructor(AccessModifier accessModifier, string name)
        {
            return new ConstructorSourceText(accessModifier, name);
        }
        
        public static IPropertySourceText CreateProperty(AccessModifier accessModifier, string type, string name, bool getOnly = false)
        {
            return new PropertySourceText(accessModifier, type, name, getOnly);
        }

        public static IFieldSourceText CreateField(AccessModifier accessModifier, string type, string name)
        {
            return new FieldSourceText(accessModifier, type, name);
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