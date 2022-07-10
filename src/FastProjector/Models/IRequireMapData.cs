using FastProjector.Models.TypeMetaDatas;

namespace FastProjector.Models
{
    internal interface IRequireMapData
    {
        public (ClassTypeMetaData sourceType, ClassTypeMetaData destinationType) GetRequiredMapTypes();
    }
}