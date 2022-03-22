using Microsoft.CodeAnalysis;

namespace FastProjector.MapGenerator.Proccessing.Models
{
    internal sealed class PrimitivePropertyTypeInformation: PropertyTypeInformation
    {
        public override PropertyTypeEnum Type { get; }
        
        public PrimitivePropertyTypeInformation(ITypeSymbol type, PropertyTypeEnum typeEnum) : base(type)
        {
            Type = typeEnum;
            
        }

    }
    
}