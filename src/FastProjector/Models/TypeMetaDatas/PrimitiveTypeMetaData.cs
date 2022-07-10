using FastProjector.Models.TypeInformations;
using Microsoft.CodeAnalysis;

namespace FastProjector.Models.TypeMetaDatas
{
    internal class PrimitiveTypeMetaData : TypeMetaData
    {
        public PrimitiveTypeMetaData(ITypeSymbol typeSymbol, PrimitiveTypeInformation typeInformation): base(typeSymbol, typeInformation)
        {
            TypeInformation = typeInformation;
        }

        public new PrimitiveTypeInformation TypeInformation { get; }
    }
}