using FastProjector.Models.TypeInformations;
using Microsoft.CodeAnalysis;
using System.Linq;

namespace FastProjector.Models.TypeMetaDatas
{
    internal class GenericCollectionTypeMetaData : CollectionTypeMetaData
    {
        public GenericCollectionTypeMetaData(ITypeSymbol typeSymbol, GenericCollectionTypeInformation typeInformation) :
            base(typeSymbol)
        {
            TypeInformation = typeInformation;
        }

        public override TypeInformation TypeInformation { get; }
        
        public override TypeMetaData GetCollectionType()
        {
            var collectionType = (TypeSymbol as INamedTypeSymbol)?.TypeArguments.First();

            return Create(collectionType);
        }
    }
}