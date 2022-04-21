using Microsoft.CodeAnalysis;

namespace FastProjector.Models.TypeMetaDatas
{
    internal abstract class CollectionTypeMetaData: TypeMetaData
    {
        protected CollectionTypeMetaData(ITypeSymbol typeSymbol) : base(typeSymbol)
        { }

        public abstract TypeMetaData GetCollectionType();
    }
}