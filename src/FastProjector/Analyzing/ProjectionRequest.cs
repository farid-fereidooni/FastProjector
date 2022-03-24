using Microsoft.CodeAnalysis;

namespace FastProjector.Analyzing
{
    internal class ProjectionRequest
    {
        public INamedTypeSymbol ProjectionSource { get; set; }
        public INamedTypeSymbol ProjectionTarget {get;set;}
    }
}