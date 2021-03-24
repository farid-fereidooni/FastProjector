using System;

namespace FastProjector.MapGenerator.Proccessing.Models
{
    internal class PropertyCastResult
    {
        
        public bool IsUnMapable {get;set;}
        public CastMetaData SourcePropertyCastMetaData {get;set;}
        public CastMetaData DestinationPropertyCastMetaData {get;set;}
        public Func<string, string> Cast {get;set;}
    }
}