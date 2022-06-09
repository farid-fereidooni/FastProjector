using Microsoft.CodeAnalysis;

namespace FastProjector.Models.TypeInformations
{
    internal class ClassTypeInformation: TypeInformation
    {
        public ClassTypeInformation(ITypeSymbol type) : base(type)
        {
            Type = PropertyType.Other;
        }

        public override PropertyType Type { get; }
    }
}