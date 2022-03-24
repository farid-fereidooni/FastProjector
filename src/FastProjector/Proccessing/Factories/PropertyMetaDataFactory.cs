using System;
using Microsoft.CodeAnalysis;

namespace FastProjector.MapGenerator.Proccessing.Models
{
    internal class PropertyMetaDataFactory : IPropertyMetaDataFactory
    {
        private readonly IPropertyTypeInformationFactory _propertyTypeFactory;

        public PropertyMetaDataFactory(IPropertyTypeInformationFactory propertyTypeFactory)
        {
            _propertyTypeFactory = propertyTypeFactory;
        }
        public PropertyMetaData CreatePropertyMetaData(IPropertySymbol propertySymbol)
        {
            var propertyTypeInformation = _propertyTypeFactory.CreatePropertyTypeInformation(propertySymbol);
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