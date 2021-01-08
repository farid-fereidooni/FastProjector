using System;
using Microsoft.CodeAnalysis;

namespace FastProjector.MapGenerator.Analyzing
{
    internal class ProjectionRequest
    {
        public INamedTypeSymbol ProjectionSource { get; set; }
        public INamedTypeSymbol ProjectionTarget {get;set;}
    }
}