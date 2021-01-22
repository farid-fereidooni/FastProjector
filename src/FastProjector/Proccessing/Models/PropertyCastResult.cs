using System;

namespace FastProjector.MapGenerator.Proccessing.Models
{
    internal class PropertyCastResult
    {
        public PropertyCastResult(bool isUnMapable, Func<string, string> cast)
        {
            IsUnMapable = isUnMapable;
            Cast = cast;
        }

        public bool IsUnMapable {get;set;}
        public Func<string, string> Cast {get;set;}
    }
}