using Microsoft.CodeAnalysis;

namespace FastProjector.Models.TypeInformations
{
    internal class GenericClassTypeInformation: TypeInformation
    {
        public override PropertyTypeEnum Type { get; }
        
        public GenericClassTypeInformation(ITypeSymbol type) : base(type)
        {
            Type = PropertyTypeEnum.Other;
        }

    }
}