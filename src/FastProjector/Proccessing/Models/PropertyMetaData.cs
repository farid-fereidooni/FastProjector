using System;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace FastProjector.MapGenerator.Proccessing.Models
{
    internal class PropertyMetaData
    {

        public PropertyMetaData(IPropertySymbol propertySymbol)
        {
            PropertySymbol = propertySymbol;
            PropertyTypeInformation = new PropertyTypeInformation(propertySymbol);
        }
        public IPropertySymbol PropertySymbol { get; }
        public PropertyTypeInformation PropertyTypeInformation { get; }

        public ITypeSymbol GetCollectionTypeSymbol()
        {
            if (!PropertyTypeInformation.IsEnumerable())
                throw new Exception("Property isn't collection");

            return  PropertyTypeInformation.Type == PropertyTypeEnum.System_Array ? 
                ((IArrayTypeSymbol) PropertySymbol.Type).ElementType
                : (PropertySymbol.Type as INamedTypeSymbol)?.TypeArguments.First();
        }
    }
}