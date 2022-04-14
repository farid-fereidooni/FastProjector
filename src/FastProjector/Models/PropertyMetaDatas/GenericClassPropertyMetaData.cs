using FastProjector.Models.PropertyTypeInformations;
using Microsoft.CodeAnalysis;

namespace FastProjector.Models.PropertyMetaDatas
{
    internal class GenericClassPropertyMetaData : PropertyMetaData
    {
        public GenericClassPropertyMetaData(IPropertySymbol propertySymbol, GenericCollectionPropertyTypeInformation propertyTypeInformation) : base(propertySymbol)
        {
            PropertyTypeInformation = propertyTypeInformation;
        }

        public override PropertyTypeInformation PropertyTypeInformation { get; }
    }
}