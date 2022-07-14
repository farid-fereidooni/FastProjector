using System.Collections.Generic;
using FastProjector.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FastProjector.Analyzing
{
    internal class ProjectionSyntaxReceiver : ISyntaxReceiver
    {
        public List<ClassDeclarationSyntax> ProjectionCandidates { get; } = new();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax &&
                classDeclarationSyntax.AttributeLists.NotNullAny(attribList =>
                    attribList.Attributes.NotNullAny(attrib =>
                        attrib.Name.NormalizeWhitespace().ToFullString() == "ProjectFrom")))
            {
                ProjectionCandidates.Add(classDeclarationSyntax);
            }
        }
    }
}