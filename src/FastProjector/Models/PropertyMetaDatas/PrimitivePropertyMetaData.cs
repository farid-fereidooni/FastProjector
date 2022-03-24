using FastProjector.Models.PropertyTypeInformations;
using Microsoft.CodeAnalysis;

namespace FastProjector.Models.PropertyMetaDatas
{
    internal class PrimitivePropertyMetaData : PropertyMetaData
    {
        public PrimitivePropertyMetaData(IPropertySymbol propertySymbol, PrimitivePropertyTypeInformation propertyTypeInformation) : base(propertySymbol)
        {
            PropertyTypeInformation = propertyTypeInformation;
        }

        public override PropertyTypeInformation PropertyTypeInformation { get; }
    }
}