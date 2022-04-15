using FastProjector.Models.TypeMetaDatas;
using Microsoft.CodeAnalysis;

namespace FastProjector.Models.PropertyMetadatas
{
    internal sealed class ClassPropertyMetadata: PropertyMetadata
    {
        public ClassPropertyMetadata(IPropertySymbol propertySymbol, ClassTypeMetaData typeMetaData) : base(propertySymbol, typeMetaData)
        {
        }
    }
}