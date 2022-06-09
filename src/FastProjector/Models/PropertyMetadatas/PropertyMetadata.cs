using System;
using FastProjector.Models.TypeMetaDatas;
using Microsoft.CodeAnalysis;

namespace FastProjector.Models.PropertyMetadatas
{
    internal abstract class PropertyMetadata
    {
        protected PropertyMetadata(IPropertySymbol propertySymbol, TypeMetaData typeMetaData)
        {
            PropertyName = propertySymbol.Name;
            TypeMetaData = typeMetaData;        
        }
        
        public TypeMetaData TypeMetaData { get; }

        public string PropertyName { get; }

        public override bool Equals(object obj)
        {
            if (obj is PropertyMetadata other)
            {
                return TypeMetaData.Equals(other.TypeMetaData) && PropertyName.Equals(other.PropertyName);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return TypeMetaData.GetHashCode();
        }

        public static PropertyMetadata Create(IPropertySymbol propertySymbol)
        {
            
            var typeMetadata = TypeMetaData.Create(propertySymbol.Type);
            return typeMetadata switch
            {
                ClassTypeMetaData classTypeMetaData => new ClassPropertyMetadata(propertySymbol, classTypeMetaData),
                PrimitiveTypeMetaData primitiveTypeMetaData => new PrimitivePropertyMetadata(propertySymbol,
                    primitiveTypeMetaData),
                _ => throw new ArgumentException(nameof(propertySymbol))
            };
        }
    }
}