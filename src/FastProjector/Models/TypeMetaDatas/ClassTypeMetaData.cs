using FastProjector.Models.TypeInformations;
using Microsoft.CodeAnalysis;

namespace FastProjector.Models.TypeMetaDatas
{
    internal class ClassTypeMetaData : TypeMetaData
    {
        public ClassTypeMetaData(ITypeSymbol typeSymbol, ClassTypeInformation typeInformation) : base(typeSymbol, typeInformation)
        {
            TypeInformation = typeInformation;
        }

        public new ClassTypeInformation TypeInformation { get; }
    }
}