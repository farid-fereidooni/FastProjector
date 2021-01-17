using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public void Execute(GeneratorExecutionContext context)
        {
            try {
                if(!(context.SyntaxReceiver is ProjectionSyntaxReceiver projectionSyntaxReceiver))
                    return;

                if (projectionSyntaxReceiver.ProjectionCandidates.NotNullAny())
                {
                    List<ProjectionRequest> requests = new List<ProjectionRequest>();
                    foreach (var projectionCandidate in projectionSyntaxReceiver.ProjectionCandidates)
                    {
                        try {
                            requests.Add(SymbolDetector.AnalyzeProjectionCandidates(projectionCandidate, context));
                        }
                        catch (ArgumentException)
                        { }
                    }

                    string finalSource = RequestProccessing.ProccessProjectionRequest(requests);
                    context.AddSource("Projections.cs", SourceText.From(finalSource, Encoding.UTF8));
                }
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