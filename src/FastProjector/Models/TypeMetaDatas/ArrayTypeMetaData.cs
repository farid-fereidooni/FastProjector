using FastProjector.Models.TypeInformations;
using Microsoft.CodeAnalysis;

namespace FastProjector.Models.TypeMetaDatas
{
    internal class ArrayTypeMetaData: CollectionTypeMetaData
    {
        public ArrayTypeMetaData(ITypeSymbol typeSymbol, ArrayTypeInformation typeInformation) : base(typeSymbol)
        {
            TypeInformation = typeInformation;
        }

        public override TypeInformation TypeInformation { get; }
        
        public override TypeMetaData GetCollectionType()
        {
            var element = (TypeSymbol as IArrayTypeSymbol)?.ElementType;
            return Create(element);
        }
    }
}