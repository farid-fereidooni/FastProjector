using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FastProjector.Analyzing
{
    internal static class SymbolDetector
    {
        
        public static ProjectionRequest AnalyzeProjectionCandidates(ClassDeclarationSyntax projectionCandidate, GeneratorExecutionContext context)
        {
            if (projectionCandidate == null) 
                throw new ArgumentNullException();
            
            var compilation = context.Compilation;
            var projectFromSymbol = compilation.GetTypeByMetadataName("FastProjector.Shared.ProjectFrom");

            var projectionCandidateModel = compilation.GetSemanticModel(projectionCandidate.SyntaxTree);
            var projectionCandidateSymbol = projectionCandidateModel.GetDeclaredSymbol(projectionCandidate) as INamedTypeSymbol;

            var projectionAttribs = projectionCandidateSymbol.GetAttributes().Where(a => a.AttributeClass.Equals(projectFromSymbol, SymbolEqualityComparer.Default)).ToList();
            
            if (!projectionAttribs.NotNullAny()) 
                throw new ArgumentException();
            
            //found using semantic API
            foreach (var projectionAttrib in projectionAttribs)
            {
                var projectionTarget = compilation.GetTypeByMetadataName(projectionAttrib.ConstructorArguments.FirstOrDefault().Value.ToString());
                if (projectionTarget != null)
                {
                    return new ProjectionRequest {
                        ProjectionSource = projectionCandidateSymbol,
                        ProjectionTarget = projectionTarget
                    };
                }
            }
            throw new ArgumentException();

        }
    }
}