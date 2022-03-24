using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using FastProjector.MapGenerator.Analyzing;
using FastProjector.MapGenerator.DevTools;
using FastProjector.MapGenerator.Proccessing;
using FastProjector.MapGenerator.Proccessing.Models;
using FastProjector.MapGenerator.Proccessing.Models.Assignments;
using FastProjector.MapGenerator.Proccessing.Services;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace FastProjector.MapGenerator
{
    [Generator]
    public class SourceAnalyzer : ISourceGenerator
    {
        private readonly IProjectionRequestProcessor _requestProcessor; 
        
        public SourceAnalyzer()
        {
            var propertyMetaDataFactory = new PropertyMetaDataFactory(new PropertyTypeInformationFactory());
            var assignmentFactory = new PropertyAssignmentFactory(new CastingService());
            var mapService = new ModelMapService(new MapCache(), propertyMetaDataFactory, assignmentFactory);
            
            _requestProcessor = new ProjectionRequestProcessor(mapService);
        }

        public void Execute(GeneratorExecutionContext context)
        {        
            try {
                if(context.SyntaxReceiver is not ProjectionSyntaxReceiver projectionSyntaxReceiver)
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
                
                var finalSource = _requestProcessor.ProcessProjectionRequest(requests);
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

            #if DEBUG
                if (!Debugger.IsAttached)
                {
                    Debugger.Launch();
                }
            
                while (!Debugger.IsAttached)
                {
                    //Debugger.Launch();
                    System.Threading.Thread.Sleep(500);
                }
            #endif
            
           // Logger.RemoveFile();
            Logger.Log("started");
            context.RegisterForSyntaxNotifications(() => new ProjectionSyntaxReceiver());
        }
   
    }
 
}