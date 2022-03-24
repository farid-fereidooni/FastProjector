using System;
using FastProjector.Models.PropertyTypeInformations;
using Microsoft.CodeAnalysis;

namespace FastProjector.Models.PropertyMetaDatas
{
    internal abstract class PropertyMetaData
    {

        public PropertyMetaData(IPropertySymbol propertySymbol)
        {
            PropertySymbol = propertySymbol ?? throw new ArgumentNullException(nameof(propertySymbol));
        }

        public IPropertySymbol PropertySymbol { get; }
        public abstract PropertyTypeInformation PropertyTypeInformation { get; }
    
        public string GetPropertyName()
        {
            return PropertySymbol.Name;
        }

        public override bool Equals(object obj)
        {
            if (obj is PropertyMetaData other)
            {
                return PropertyTypeInformation.Equals(other.PropertyTypeInformation);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return PropertyTypeInformation.GetHashCode();
        }
    
        // public ITypeSymbol GetCollectionTypeSymbol()
        // {
        //     if (!PropertyTypeInformation.IsEnumerable())
        //         throw new Exception("Property isn't collection");
        //
        //     return  PropertyTypeInformation.Type == PropertyTypeEnum.System_Array ? 
        //         ((IArrayTypeSymbol) PropertySymbol.Type).ElementType
        //         : (PropertySymbol.Type as INamedTypeSymbol)?.TypeArguments.First();
        // }
        
        public static PropertyMetaData CreatePropertyMetaData(IPropertySymbol propertySymbol)
        {
            var propertyTypeInformation = PropertyTypeInformation.CreatePropertyTypeInformation(propertySymbol);
            if (propertyTypeInformation is null)
                return null;

            return propertyTypeInformation switch
            {
                ClassPropertyTypeInformation classType => null,
                ArrayPropertyTypeInformation arrayType => null,
                GenericCollectionPropertyTypeInformation genericCollectionType => null,
                GenericClassPropertyTypeInformation genericClassType => null,
                PrimitivePropertyTypeInformation primitiveType => new PrimitivePropertyMetaData(propertySymbol,
                    primitiveType),
                _ => null
            };
        }
    }
}