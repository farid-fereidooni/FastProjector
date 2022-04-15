using FastProjector.Models.TypeMetaDatas;

namespace FastProjector.Models
{
    internal class PropertyMapMetaData
    {
        public PropertyMapMetaData(TypeMetaData sourceType, TypeMetaData destinationType)
        {
            SourceType = sourceType;
            DestinationType = destinationType;
        }
        public TypeMetaData SourceType { get; }
        public TypeMetaData DestinationType { get; }
    }
}