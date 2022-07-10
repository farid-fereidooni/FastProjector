using Microsoft.CodeAnalysis;

namespace FastProjector.Models.TypeInformations
{
    internal class GenericClassTypeInformation: ClassTypeInformation
    {
        public GenericClassTypeInformation(ITypeSymbol type) : base(type)
        { }
    }
}