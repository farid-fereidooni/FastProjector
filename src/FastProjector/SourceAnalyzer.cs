using System.Diagnostics;
using FastProjector.MapGenerator.Analyzing;
using FastProjector.MapGenerator.DevTools;
using Microsoft.CodeAnalysis;

namespace FastProjector.MapGenerator
{
    [Generator]
    public class SourceAnalyzer : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {

            if(!(context.SyntaxReceiver is ProjectionSyntaxReceiver projectionSyntaxReceiver))
                return;

            SymbolProccessor.ProcessProjectionRequests(projectionSyntaxReceiver.ProjectionCandidates, context);
        
        }

        public void Initialize(GeneratorInitializationContext context)
        {
             Logger.RemoveFile();

            context.RegisterForSyntaxNotifications(() => new ProjectionSyntaxReceiver());
        }
    }
}