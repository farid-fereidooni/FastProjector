using FastProjector.Models.TypeMetaDatas;
using Microsoft.CodeAnalysis;

namespace FastProjector.Models.PropertyMetadatas
{
    internal class CollectionPropertyMetadata: PropertyMetadata
    {
        public CollectionPropertyMetadata(IPropertySymbol propertySymbol, CollectionTypeMetaData typeMetaData) : base(propertySymbol, typeMetaData)
        {
            TypeMetaData = typeMetaData;
        }
        
        public new CollectionTypeMetaData TypeMetaData { get; }

    }
}