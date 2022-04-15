using FastProjector.Models.TypeMetaDatas;
using Microsoft.CodeAnalysis;

namespace FastProjector.Models.PropertyMetadatas
{
    internal sealed class PrimitivePropertyMetadata: PropertyMetadata
    {
        public PrimitivePropertyMetadata(IPropertySymbol propertySymbol, PrimitiveTypeMetaData typeMetaData) : base(propertySymbol, typeMetaData)
        {
        }
    }
}