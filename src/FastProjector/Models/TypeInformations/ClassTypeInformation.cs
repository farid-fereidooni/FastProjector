using Microsoft.CodeAnalysis;

namespace FastProjector.Models.TypeInformations
{
    internal class ClassTypeInformation: TypeInformation
    {
        public ClassTypeInformation(ITypeSymbol type) : base(type)
        {
            Type = PropertyTypeEnum.Other;
        }

        public override PropertyTypeEnum Type { get; }
    }
}