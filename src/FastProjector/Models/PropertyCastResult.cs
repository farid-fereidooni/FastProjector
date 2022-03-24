using System;
using FastProjector.Models.PropertyTypeInformations;

namespace FastProjector.Models
{
    internal class PropertyCastResult
    {
        
        public bool IsUnMapable {get;set;}
        public PropertyTypeInformation SourceProperyTypeInfo {get;set;}
        public PropertyTypeInformation DestinationProperyTypeInfo {get;set;}
        public Func<string, string> Cast {get;set;}
    }
}