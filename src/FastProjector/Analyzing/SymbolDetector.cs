using System;
using System.Collections.Generic;
using System.Linq;
using FastProjector.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FastProjector.Analyzing
{
    internal static class SymbolDetector
    {
        public static IEnumerable<ProjectionRequest> AnalyzeProjectionCandidates(
            ClassDeclarationSyntax projectionCandidate, GeneratorExecutionContext context)
        {
            if (projectionCandidate == null) throw new ArgumentNullException(nameof(projectionCandidate));

            var compilation = context.Compilation;
            var projectFromSymbol = compilation.GetTypeByMetadataName("FastProjector.Shared.ProjectFrom");

            var projectionCandidateModel = compilation.GetSemanticModel(projectionCandidate.SyntaxTree);
            var projectionCandidateSymbol =
                projectionCandidateModel.GetDeclaredSymbol(projectionCandidate) as INamedTypeSymbol;

            var projectionAttribs = projectionCandidateSymbol.GetAttributes().Where(a =>
                a.AttributeClass.Equals(projectFromSymbol, SymbolEqualityComparer.Default)).ToList();

            if (!projectionAttribs.NotNullAny())
                throw new ArgumentException("Requested candidate doest not uses ProjectFrom attribute ");

            return ExtractProjectionRequests(projectionAttribs, compilation, projectionCandidateSymbol);
        }

        private static IEnumerable<ProjectionRequest> ExtractProjectionRequests(List<AttributeData> projectionAttribs,
            Compilation compilation,
            INamedTypeSymbol projectionCandidateSymbol)
        {
            foreach (var projectionAttrib in projectionAttribs)
            {
                var projectionSource =
                    compilation.GetTypeByMetadataName(projectionAttrib.ConstructorArguments.FirstOrDefault().Value
                        .ToString());
                if (projectionSource == null) continue;
                yield return new ProjectionRequest
                {
                    ProjectionSource = projectionSource,
                    ProjectionTarget = projectionCandidateSymbol
                };
            }
        }
    }
}