using System;
using System.Collections.Generic;
using System.Text;
using FastProjector.Analyzing;
using FastProjector.Contracts;
using FastProjector.Helpers;
using FastProjector.Ioc;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace FastProjector
{
    [Generator]
    public class SourceAnalyzer : ISourceGenerator
    {
        private readonly IProjectionRequestProcessor _requestProcessor; 
        
        public SourceAnalyzer()
        {
            IContainer container = new IocContainer();
            container.AddServices();
            var scope =  container.CreateScope();
            _requestProcessor = scope.GetService<IProjectionRequestProcessor>();
        }

        public void Execute(GeneratorExecutionContext context)
        {        
          
            if(context.SyntaxReceiver is not ProjectionSyntaxReceiver projectionSyntaxReceiver)
                return;

            if (!projectionSyntaxReceiver.ProjectionCandidates.NotNullAny()) return;
            
            var requests = new List<ProjectionRequest>();
            foreach (var projectionCandidate in projectionSyntaxReceiver.ProjectionCandidates)
            {
                requests.AddRange(SymbolDetector.AnalyzeProjectionCandidates(projectionCandidate, context));
            }
            
            var finalSource = _requestProcessor.ProcessProjectionRequest(requests);
            context.AddSource("Projections.cs", SourceText.From(finalSource, Encoding.UTF8));
            

        }

        public void Initialize(GeneratorInitializationContext context)
        {
/*
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
            */
            
            context.RegisterForSyntaxNotifications(() => new ProjectionSyntaxReceiver());
        }
    }
}