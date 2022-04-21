using System;
using FastProjector.Models;
using FastProjector.Models.TypeInformations;

namespace FastProjector.Helpers
{
    internal static class PropertyTypeInfoExtentions
    {
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
                throw new ArgumentException("Invalid Type");

            return propertyTypeEnum;
        }
    }
}