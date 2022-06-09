using Microsoft.CodeAnalysis;

namespace FastProjector.Models.TypeInformations
{
    internal class GenericClassTypeInformation: TypeInformation
    {
        public override PropertyType Type { get; }
        
        public GenericClassTypeInformation(ITypeSymbol type) : base(type)
        {
            Type = PropertyType.Other;
        }

    }
}