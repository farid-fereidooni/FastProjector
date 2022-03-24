using Microsoft.CodeAnalysis;

namespace FastProjector.Models.PropertyTypeInformations
{
    internal class ClassPropertyTypeInformation: PropertyTypeInformation
    {
        public ClassPropertyTypeInformation(ITypeSymbol type) : base(type)
        {
            Type = PropertyTypeEnum.Other;
        }

        public override PropertyTypeEnum Type { get; }
    }
}