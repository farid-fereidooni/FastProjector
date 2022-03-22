using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace FastProjector.MapGenerator.Proccessing.Models
{
    internal abstract class PropertyTypeInformation : TypeInformation
    {

        public PropertyTypeInformation(ITypeSymbol type)
            : base(type)
        { }
        


        public abstract PropertyTypeEnum Type { get; }
        
        // public static PropertyTypeInformation CreatePrimitive(PrimitiveTypeEnum primitiveType)
        // {
        //     if (!Enum.IsDefined(typeof(PrimitiveTypeEnum), primitiveType))
        //         throw new InvalidEnumArgumentException(nameof(primitiveType), (int) primitiveType,
        //             typeof(PrimitiveTypeEnum));
        //     
        //     var propertyType = primitiveType.ConvertToPropertyTypeEnum();
        //     
        //     var fullName = propertyType.GetFullname();
        //     return new PropertyTypeInformation(fullName, null, propertyType, PropertyTypeCategoryEnum.SinglePrimitive,
        //         null);
        // }
        //
        // public static PropertyTypeInformation CreateClass(string fullname)
        // {
        //     return new PropertyTypeInformation(fullname, null, PropertyTypeEnum.Other,
        //         PropertyTypeCategoryEnum.SingleNonGenenericClass, null);
        // }
        //
        // public static PropertyTypeInformation CreateGenericClass(string fullname, IEnumerable<SubTypeInformation> genericTypes)
        // {
        //         return new PropertyTypeInformation(fullname, genericTypes, PropertyTypeEnum.Other,
        //                     PropertyTypeCategoryEnum.SingleGenericClass, null);
        // }
        //
        // public static PropertyTypeInformation CreatePrimitiveArray(PrimitiveTypeEnum arrayType)
        // {
        //     if (!Enum.IsDefined(typeof(PrimitiveTypeEnum), arrayType))
        //         throw new InvalidEnumArgumentException(nameof(arrayType), (int) arrayType,
        //             typeof(PrimitiveTypeEnum));
        //
        //     var propertyType = arrayType.ConvertToPropertyTypeEnum();
        //     
        //     var fullName = propertyType.GetFullname();
        //
        //     return new PropertyTypeInformation("System.Array", null, PropertyTypeEnum.System_Array,
        //         PropertyTypeCategoryEnum.CollectionPrimitive, new SubTypeInformation(fullName));
        // }
        //
        // public static PropertyTypeInformation CreateObjectArray(string arrayTypeFullname)
        // {
        //     return new PropertyTypeInformation("System.Array", null, PropertyTypeEnum.System_Array,
        //         PropertyTypeCategoryEnum.CollectionObject, new SubTypeInformation(arrayTypeFullname));
        // }
        //
        //
        // public static PropertyTypeInformation CreatePrimitiveCollection(CollectionTypeEnum collectionCategoryType, PrimitiveTypeEnum collectionType)
        // {
        //     if (!Enum.IsDefined(typeof(CollectionTypeEnum), collectionCategoryType))
        //         throw new InvalidEnumArgumentException(nameof(collectionCategoryType), (int) collectionCategoryType,
        //             typeof(CollectionTypeEnum));
        //     
        //     if (!Enum.IsDefined(typeof(PrimitiveTypeEnum), collectionType))
        //         throw new InvalidEnumArgumentException(nameof(collectionType), (int) collectionType,
        //             typeof(PrimitiveTypeEnum));
        //
        //     var categoryPropertyType = collectionCategoryType.ConvertToPropertyTypeEnum();
        //     var genericPropertyType = collectionType.ConvertToPropertyTypeEnum();
        //     
        //     var categoryFullName = categoryPropertyType.GetFullname();
        //     var genericFullName = genericPropertyType.GetFullname();
        //     var genericType = new SubTypeInformation(genericFullName);
        //
        //     return new PropertyTypeInformation(categoryFullName, new []{ genericType }, categoryPropertyType,
        //         PropertyTypeCategoryEnum.CollectionPrimitive, null);
        // }
        //
        // public static PropertyTypeInformation CreateObjectCollection(CollectionTypeEnum collectionCategoryType, string collectionTypeFullName)
        // {
        //     if (!Enum.IsDefined(typeof(CollectionTypeEnum), collectionCategoryType))
        //         throw new InvalidEnumArgumentException(nameof(collectionCategoryType), (int) collectionCategoryType,
        //             typeof(CollectionTypeEnum));
        //
        //     var categoryPropertyType = collectionCategoryType.ConvertToPropertyTypeEnum();
        //
        //     var categoryFullName = categoryPropertyType.GetFullname();
        //     var genericType = new SubTypeInformation(collectionTypeFullName);
        //
        //
        //     return new PropertyTypeInformation(categoryFullName, new []{ genericType }, categoryPropertyType,
        //         PropertyTypeCategoryEnum.CollectionObject, null);
        // }
        
    }
}