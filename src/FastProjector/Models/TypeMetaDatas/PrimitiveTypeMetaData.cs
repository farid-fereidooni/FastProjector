using FastProjector.Models.TypeInformations;
using Microsoft.CodeAnalysis;

namespace FastProjector.Models.TypeMetaDatas
{
    internal class PrimitiveTypeMetaData : TypeMetaData
    {
        public PrimitiveTypeMetaData(ITypeSymbol typeSymbol, PrimitiveTypeInformation typeInformation): base(typeSymbol)
        {
            TypeInformation = typeInformation;
        }

        public override TypeInformation TypeInformation { get; }
    }
}