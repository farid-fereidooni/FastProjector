using FastProjector.Models.TypeInformations;
using Microsoft.CodeAnalysis;

namespace FastProjector.Models.TypeMetaDatas
{
    internal class GenericClassTypeMetaData : ClassTypeMetaData
    {
        public GenericClassTypeMetaData(ITypeSymbol typeSymbol, GenericClassTypeInformation typeInformation) : base(typeSymbol, typeInformation)
        {
            TypeInformation = typeInformation;
        }

        public new GenericClassTypeInformation TypeInformation { get; }
    }
}