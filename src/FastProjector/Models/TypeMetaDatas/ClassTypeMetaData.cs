using FastProjector.Models.TypeInformations;
using Microsoft.CodeAnalysis;

namespace FastProjector.Models.TypeMetaDatas
{
    internal class ClassTypeMetaData : TypeMetaData
    {
        public ClassTypeMetaData(ITypeSymbol typeSymbol, ClassTypeInformation typeInformation) : base(typeSymbol)
        {
            TypeInformation = typeInformation;
        }

        public override TypeInformation TypeInformation { get; }
    }
}