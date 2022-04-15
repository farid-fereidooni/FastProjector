using System;
using FastProjector.Models.TypeInformations;

namespace FastProjector.Models
{
    internal class PropertyCastResult
    {
        
        public bool IsUnMapable {get;set;}
        public TypeInformation SourceProperyTypeInfo {get;set;}
        public TypeInformation DestinationProperyTypeInfo {get;set;}
        public Func<string, string> Cast {get;set;}
    }
}