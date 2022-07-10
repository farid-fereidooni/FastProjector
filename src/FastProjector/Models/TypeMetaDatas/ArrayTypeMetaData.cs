using FastProjector.Models.TypeInformations;
using Microsoft.CodeAnalysis;

namespace FastProjector.Models.TypeMetaDatas
{
    internal class ArrayTypeMetaData: CollectionTypeMetaData
    {
        public ArrayTypeMetaData(ITypeSymbol typeSymbol, ArrayTypeInformation typeInformation) : base(typeSymbol, typeInformation)
        { }
        
        public override TypeMetaData GetCollectionType()
        {
            var element = (TypeSymbol as IArrayTypeSymbol)?.ElementType;
            return Create(element);
        }

    }
}