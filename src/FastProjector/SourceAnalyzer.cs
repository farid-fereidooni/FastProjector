using System;
using System.Collections.Generic;
using System.Text;
using FastProjector.MapGenerator.Analyzing;
using FastProjector.MapGenerator.DevTools;
using FastProjector.MapGenerator.Proccessing;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace FastProjector.MapGenerator
{
    [Generator]
    public class SourceAnalyzer : ISourceGenerator
    {
        private readonly RequestProcessing requestProcessing;
        public SourceAnalyzer()
        {
            var mapCache = new MapCache();
            var propertyCasting = new PropertyCasting();
            requestProcessing = new RequestProcessing(mapCache, propertyCasting);
        }
        public void Execute(GeneratorExecutionContext context)
        {
            
            try {
                if(!(context.SyntaxReceiver is ProjectionSyntaxReceiver projectionSyntaxReceiver))
                    return;

                if (!projectionSyntaxReceiver.ProjectionCandidates.NotNullAny()) return;
                
                var requests = new List<ProjectionRequest>();
                foreach (var projectionCandidate in projectionSyntaxReceiver.ProjectionCandidates)
                {
                    try {
                        requests.Add(SymbolDetector.AnalyzeProjectionCandidates(projectionCandidate, context));
                    }
                    catch (ArgumentException)
                    { }
                }

                var finalSource = requestProcessing.ProcessProjectionRequest(requests);
                context.AddSource("Projections.cs", SourceText.From(finalSource, Encoding.UTF8));
            }
            catch (Exception ex) {
                Logger.Log(ex.Message);
                Logger.Log(ex.StackTrace);
                throw;
            }

        }

        public void Initialize(GeneratorInitializationContext context)
        {
                
            
             Logger.RemoveFile();

            context.RegisterForSyntaxNotifications(() => new ProjectionSyntaxReceiver());
        }
    }
}