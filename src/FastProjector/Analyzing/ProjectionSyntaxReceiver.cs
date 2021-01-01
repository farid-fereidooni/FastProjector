using FastProjector.MapGenerator.DevTools;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FastProjector.MapGenerator.Analyzing
{
    internal class ProjectionSyntaxReceiver : ISyntaxReceiver
    {
        public List<ClassDeclarationSyntax> ProjectionCandidates { get; set; } = new List<ClassDeclarationSyntax>();
        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
 
            if(syntaxNode is ClassDeclarationSyntax classDeclarationSyntax &&
                classDeclarationSyntax.AttributeLists.NotNullAny(attribList => 
                attribList.Attributes.NotNullAny(attrib => attrib.Name.NormalizeWhitespace().ToFullString() == "ProjectFrom")))
                {
                    //only name has checked in this step, next it's important to double check it with semantic api.
                    ProjectionCandidates.Add(classDeclarationSyntax);
                }
      
        }
    }

}