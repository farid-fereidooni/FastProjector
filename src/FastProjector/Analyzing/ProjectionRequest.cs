using System;

namespace FastProjector.MapGenerator.Analyzing
{
    internal class ProjectionRequest
    {
        public Type ProjectionSource { get; set; }
        public Type ProjectionTarget {get;set;}
    }
}