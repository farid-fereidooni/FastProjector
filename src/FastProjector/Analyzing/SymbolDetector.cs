using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System.Diagnostics;
using FastProjector.MapGenerator.DevTools;

namespace FastProjector.MapGenerator.Analyzing
{
    internal static class SymbolProccessor
    {
        public static List<ProjectionRequest> ProcessProjectionRequests(List<ClassDeclarationSyntax> projectionCandidates, GeneratorExecutionContext context)
        {
            List<ProjectionRequest> result = new List<ProjectionRequest>();
            if(projectionCandidates != null && projectionCandidates.Any())
            {
                Logger.Log("Candidate is not null");
                var compilation = context.Compilation;
                var projectFromSymbol = compilation.GetTypeByMetadataName("FastProjector.Shared.ProjectFrom");

                foreach(var projectionCandidateItem in projectionCandidates)
                {
                Logger.Log("Iterating candidates");
                    var projectionCandidateModel = compilation.GetSemanticModel(projectionCandidateItem.SyntaxTree);
                    var projectionCandidateSymbol = projectionCandidateModel.GetDeclaredSymbol(projectionCandidateItem) as ITypeSymbol;

                    var projectionAttribs = projectionCandidateSymbol.GetAttributes().Where(a => a.AttributeClass.Equals(projectFromSymbol, SymbolEqualityComparer.Default)).ToList();
                    if (projectionAttribs != null && projectionAttribs.Any())
                    {
                        //found using semantic API
                        foreach(var projectionAttrib in projectionAttribs)
                        {
                            Logger.Log(projectionCandidateSymbol.Name);
                        }
                    }

                    
                }
            }else {
                Logger.Log("no candidate!");
            }
            return result;
        }
    }
}