using System;
using FastProjector.MapGenerator.Proccessing.Models;

namespace FastProjector.MapGenerator.Proccessing
{
    internal static class PropertyTypeInfoExtentions
    {
        public static bool IsEnumerable(this PropertyTypeInformation prop)
        {
            return prop.TypeCategory == PropertyTypeCategoryEnum.CollectionObject ||
                   prop.TypeCategory == PropertyTypeCategoryEnum.CollectionPrimitive;
        }
        
        public static bool IsCollectionObject(this PropertyTypeInformation prop)
        {
            return prop.TypeCategory == PropertyTypeCategoryEnum.CollectionObject;
        }
        
        public static bool IsNonGenericClass(this PropertyTypeInformation prop)
        {
            return prop.TypeCategory == PropertyTypeCategoryEnum.SingleNonGenenericClass;
        }

        public static bool HasGenericTypes(this TypeInformation prop)
        {
            return prop.GenericTypes.NotNullAny();
        }

        public static PropertyTypeEnum ConvertToPropertyTypeEnum(this PrimitiveTypeEnum primitiveType)
        {
            return ConvertAnyToPropertyTypeEnum(primitiveType);
        }
        
        public static PropertyTypeEnum ConvertToPropertyTypeEnum(this CollectionTypeEnum primitiveType)
        {
            return ConvertAnyToPropertyTypeEnum(primitiveType);
        }
        
        public static string GetFullname(this PropertyTypeEnum propertyType)
        {
            return propertyType.ToString().Replace('_', '.');
        }

        private static PropertyTypeEnum ConvertAnyToPropertyTypeEnum(Enum anyEnum)
        {
            if (!Enum.TryParse(anyEnum.ToString(), true, out PropertyTypeEnum propertyTypeEnum))
                throw new Exception("Invalid primitive Type");

            return propertyTypeEnum;
        }
        
    
        
        
    }
}