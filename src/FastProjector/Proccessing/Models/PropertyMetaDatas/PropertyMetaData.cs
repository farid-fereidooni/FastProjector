using System;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace FastProjector.MapGenerator.Proccessing.Models
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
    }
}