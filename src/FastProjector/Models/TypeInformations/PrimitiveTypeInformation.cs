using FastProjector.Helpers;
using Microsoft.CodeAnalysis;

namespace FastProjector.Models.TypeInformations
{
    internal sealed class PrimitiveTypeInformation: TypeInformation
    {
        public override PropertyType Type { get; }
        
        public PrimitiveTypeInformation(ITypeSymbol typeSymbol, PropertyType type) : base(typeSymbol)
        {
            Type = type;
        }
        
        public PrimitiveTypeInformation(PrimitiveTypeEnum typeEnum) : base(typeEnum.ConvertToPropertyTypeEnum().GetFullname(), null)
        {
            Type = typeEnum.ConvertToPropertyTypeEnum();
            
        }
    }
}