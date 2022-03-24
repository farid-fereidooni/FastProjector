using FastProjector.Models.PropertyTypeInformations;
using Microsoft.CodeAnalysis;

namespace FastProjector.Models.PropertyMetaDatas
{
    internal class ClassPropertyMetaData : PropertyMetaData
    {
        public ClassPropertyMetaData(IPropertySymbol propertySymbol, ClassPropertyTypeInformation propertyTypeInformation) : base(propertySymbol)
        {
            PropertyTypeInformation = propertyTypeInformation;
        }

        public override PropertyTypeInformation PropertyTypeInformation { get; }
    }
}