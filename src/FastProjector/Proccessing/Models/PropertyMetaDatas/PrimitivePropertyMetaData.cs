using Microsoft.CodeAnalysis;

namespace FastProjector.MapGenerator.Proccessing.Models
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