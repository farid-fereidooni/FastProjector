using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using FastProjector.MapGenerator.DevTools;
using System;

namespace FastProjector.MapGenerator.Analyzing
{
    internal static class SymbolDetector
    {
        
        public static ProjectionRequest AnalyzeProjectionCandidates(ClassDeclarationSyntax projectionCandidate, GeneratorExecutionContext context)
        {
            if (projectionCandidate != null )
            {
                var compilation = context.Compilation;
                var projectFromSymbol = compilation.GetTypeByMetadataName("FastProjector.Shared.ProjectFrom");

                    var projectionCandidateModel = compilation.GetSemanticModel(projectionCandidate.SyntaxTree);
                    var projectionCandidateSymbol = projectionCandidateModel.GetDeclaredSymbol(projectionCandidate) as INamedTypeSymbol;

                    var projectionAttribs = projectionCandidateSymbol.GetAttributes().Where(a => a.AttributeClass.Equals(projectFromSymbol, SymbolEqualityComparer.Default)).ToList();
                    if (projectionAttribs.NotNullAny())
                    {
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
                    }
                    throw new ArgumentException();                
            }
            else
            {
                throw new ArgumentNullException();
            }
            
        }
    }
}