using System;
using FastProjector.Models.TypeInformations;

namespace FastProjector.Models.Casting
{
    internal class PropertyCastResult
    {
        public bool IsUnMappable { get; set; }
        public TypeInformation SourcePropertyTypeInfo { get; set; }
        public TypeInformation DestinationPropertyTypeInfo { get; set; }
        public Func<string, string> Cast { get; set; }
    }
}