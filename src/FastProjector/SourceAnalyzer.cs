using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using FastProjector.MapGenerator.Analyzing;
using FastProjector.MapGenerator.DevTools;
using FastProjector.MapGenerator.Ioc;
using FastProjector.MapGenerator.Proccessing;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.DependencyInjection;

namespace FastProjector.MapGenerator
{
    [Generator]
    public class SourceAnalyzer : ISourceGenerator
    {
        private readonly IServiceProvider _serviceProvider;
        public SourceAnalyzer()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddServices();
            _serviceProvider = serviceCollection.BuildServiceProvider();
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

                using var scope = _serviceProvider.CreateScope();
                var projectionProcessor = scope.ServiceProvider.GetRequiredService<IProjectionRequestProcessor>();

                var finalSource = projectionProcessor.ProcessProjectionRequest(requests);
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
            
            Logger.RemoveFile();
             Logger.Log("started");
            context.RegisterForSyntaxNotifications(() => new ProjectionSyntaxReceiver());
        }
    }
}