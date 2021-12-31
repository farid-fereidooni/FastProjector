using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Xsl;
using FastProjector.MapGenerator.SourceGeneration.Core;
using FastProjector.MapGenerator.SourceGeneration.Interfaces;
using FastProjector.SourceGeneration.Core;

namespace FastProjector.MapGenerator.SourceGeneration
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

        public IAssignmentSourceText CreateAssignment(ISourceText left, ISourceText right)
        {
            return new AssignmentSourceText(left, right);
        }

        public IMemberInitSourceText CreateMemberInit(params IAssignmentSourceText[] assignments)
        {
            return new MemberInitSourceText(assignments);
        }
        
        public IMemberInitSourceText CreateMemberInit(IEnumerable<IAssignmentSourceText> assignments)
        {
            return new MemberInitSourceText(assignments);
        }

        public ICallSourceText CreateCall(string methodName)
        {
            return new CallSourceText(methodName);
        }
        
        public ILambdaExpressionSourceText CreateLambdaExpression()
        {
            return new LambdaExpressionSourceText();
        }
        
        public ISourceText CreateSource(string source)
        {
            return new SourceText(source);
        }

    }
}