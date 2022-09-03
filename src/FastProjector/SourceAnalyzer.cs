using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Text;
using FastProjector.Analyzing;
using FastProjector.Contracts;
using FastProjector.Helpers;
using FastProjector.Ioc;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using static FastProjector.Constants.Constants;

namespace FastProjector
{
    [Generator]
    public class SourceAnalyzer : ISourceGenerator
    {
        private readonly IProjectionRequestProcessor _requestProcessor;
        private readonly IProjectionInitializerGenerator _projectionInitializerGenerator;

        public SourceAnalyzer()
        {
            IContainer container = new IocContainer();
            container.AddServices();
            var scope = container.CreateScope();
            _requestProcessor = scope.GetService<IProjectionRequestProcessor>();
            _projectionInitializerGenerator = scope.GetService<IProjectionInitializerGenerator>();
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (context.SyntaxReceiver is not ProjectionSyntaxReceiver projectionSyntaxReceiver)
                return;

            if (!projectionSyntaxReceiver.ProjectionCandidates.NotNullAny()) return;

            var requests = new List<ProjectionRequest>();
            foreach (var projectionCandidate in projectionSyntaxReceiver.ProjectionCandidates)
            {
                requests.AddRange(SymbolDetector.AnalyzeProjectionCandidates(projectionCandidate, context));
            }

            _requestProcessor.ProcessProjectionRequest(requests);

            var namespaceSource = _projectionInitializerGenerator.Generate();

            context.AddSource($"{ProjectionInitializerClassName}.cs",
                SourceText.From(namespaceSource.Text, Encoding.UTF8));
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            // #if DEBUG
            //     if (!Debugger.IsAttached)
            //     {
            //         Debugger.Launch();
            //     }
            //
            //     while (!Debugger.IsAttached)
            //     {
            //         //Debugger.Launch();
            //         System.Threading.Thread.Sleep(500);  
            //     }
            // #endif
            //

            context.RegisterForSyntaxNotifications(() => new ProjectionSyntaxReceiver());
        }
    }
}