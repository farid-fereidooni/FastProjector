using FastProjector.Models.TypeInformations;
using Microsoft.CodeAnalysis;

namespace FastProjector.Models.TypeMetaDatas
{
    internal abstract class CollectionTypeMetaData: TypeMetaData
    {
        protected CollectionTypeMetaData(ITypeSymbol typeSymbol, CollectionTypeInformation typeInformation) : base(
            typeSymbol, typeInformation)
        {
            TypeInformation = typeInformation;
        }

        public abstract TypeMetaData GetCollectionType();
        
        public new CollectionTypeInformation TypeInformation { get; }

    }
}