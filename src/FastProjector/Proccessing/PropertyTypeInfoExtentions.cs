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

        public static bool HasGenericTypes(this TypeInformation prop)
        {
            return prop.GenericTypes.NotNullAny();
        }
    }
}