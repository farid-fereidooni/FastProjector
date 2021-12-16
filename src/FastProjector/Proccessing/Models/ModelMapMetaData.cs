using System.Collections.Generic;
using System.Linq;
using FastProjector.MapGenerator.SourceGeneration.Interfaces;
using Microsoft.CodeAnalysis;

namespace FastProjector.MapGenerator.Proccessing.Models
{
    internal class ModelMapMetaData
    {
        public ModelMapMetaData(TypeInformation sourceType,
            TypeInformation destinationType,
            ISourceText source,
            IEnumerable<PropertyMapMetaData> notMappedProps,
            int mapLevel)
        {
            SourceType = sourceType;
            DestinationType = destinationType;
            ModelMappingSource = source;
            NotMappedPropertiesMetaData = notMappedProps.ToList();
            MapLevel = mapLevel;
        }
        
        public int MapLevel { get; }
        public TypeInformation SourceType { get;  }
        public TypeInformation DestinationType { get; }
        public ISourceText ModelMappingSource { get; }

        public IReadOnlyCollection<PropertyMapMetaData> NotMappedPropertiesMetaData { get; }
        
        
        public bool Is(PropertyTypeInformation sourceType, PropertyTypeInformation destinationType)
        {
            return SourceType.IsSameAs(sourceType) &&
                   DestinationType.IsSameAs(destinationType);
        }
    }
}