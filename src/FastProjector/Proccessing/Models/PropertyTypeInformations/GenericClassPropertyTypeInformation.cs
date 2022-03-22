using Microsoft.CodeAnalysis;

namespace FastProjector.MapGenerator.Proccessing.Models
{
    internal class GenericClassPropertyTypeInformation: PropertyTypeInformation
    {
        public override PropertyTypeEnum Type { get; }
        
        public GenericClassPropertyTypeInformation(ITypeSymbol type) : base(type)
        {
            Type = PropertyTypeEnum.Other;
        }

    }
}