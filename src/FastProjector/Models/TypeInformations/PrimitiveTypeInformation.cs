using FastProjector.Helpers;
using Microsoft.CodeAnalysis;

namespace FastProjector.Models.TypeInformations
{
    internal sealed class PrimitiveTypeInformation: TypeInformation
    {
        public override PropertyTypeEnum Type { get; }
        
        public PrimitiveTypeInformation(ITypeSymbol type, PropertyTypeEnum typeEnum) : base(type)
        {
            Type = typeEnum;
        }
        
        public PrimitiveTypeInformation(PrimitiveTypeEnum typeEnum) : base(typeEnum.ConvertToPropertyTypeEnum().GetFullname(), null)
        {
            Type = typeEnum.ConvertToPropertyTypeEnum();
            
        }
    }
}