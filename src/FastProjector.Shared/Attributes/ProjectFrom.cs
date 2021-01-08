using System;

namespace FastProjector.Shared
{


    [System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class ProjectFrom : Attribute
    {
        
        public readonly Type projectionSource;
        
        // This is a positional argument
        public ProjectFrom(Type projectionSource)
        {
            this.projectionSource = projectionSource;
            
        }
    }
}