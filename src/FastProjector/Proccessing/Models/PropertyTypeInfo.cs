using System;
using System.Linq;
using FastProjector.MapGenerator.Proccessing.Models;
using Microsoft.CodeAnalysis;

namespace FastProjector.MapGenerator.Proccessing.Models
{
    internal class PropertyTypeInformation : TypeInformation
    {
        public PropertyTypeInformation(IPropertySymbol prop)
            : base(prop.Type)
        {
            CreatePropertyType(prop);
        }

        public PropertyTypeEnum Type { get; private set; }

        public PropertyTypeCategoryEnum TypeCategory { get; private set; }

        public IPropertySymbol PropertySymbol { get; private set; }

        public SubTypeInformation ArrayType { get; private set; }

        public SubTypeInformation GetCollectionType()
        {
            if (TypeCategory != PropertyTypeCategoryEnum.CollectionObject
                && TypeCategory != PropertyTypeCategoryEnum.CollectionPrimitive)
            {
                throw new Exception("Type is not enumerable");
            }

            return ArrayType ?? GenericTypes.FirstOrDefault();
        }


        private void CreatePropertyType(IPropertySymbol prop)
        {
            var typeMetadata = DeterminePropertyType(prop.Type);
            Type = typeMetadata.Type;
            PropertySymbol = prop;

            // Enumerables:
            if (typeMetadata.IsEnumerable)
            {
                ITypeSymbol collectionSymbol = null;

                // array:
                if (typeMetadata.Type == PropertyTypeEnum.System_Array)
                {
                    collectionSymbol = ((IArrayTypeSymbol) prop.Type).ElementType;
                    ArrayType = new SubTypeInformation(collectionSymbol);
                }
                //other enumerable types
                else
                {
                    collectionSymbol = GenericTypes.FirstOrDefault()?.TypeSymbol;
                }

                var enumerableArgumentTypeInfo = DeterminePropertyType(collectionSymbol);

                TypeCategory = enumerableArgumentTypeInfo.IsPrimitive
                    ? PropertyTypeCategoryEnum.CollectionPrimitive
                    : PropertyTypeCategoryEnum.CollectionObject;

                return;
            }

            // genericClass:
            if (typeMetadata.IsGenericClass)
            {
                TypeCategory = PropertyTypeCategoryEnum.SingleGenericClass;
                return;
            }

            // primitive:
            if (typeMetadata.IsPrimitive)
            {
                TypeCategory = PropertyTypeCategoryEnum.SinglePrimitive;
                return;
            }

            // single class or unknown

            TypeCategory = typeMetadata.IsNonGenericClass
                ? PropertyTypeCategoryEnum.SingleNonGenenericClass
                : PropertyTypeCategoryEnum.Unknown;
        }


        private PropertyTypeMetadata DeterminePropertyType(ITypeSymbol type)
        {
            var metadata = new PropertyTypeMetadata();

            //if array:
            if (type is IArrayTypeSymbol)
            {
                metadata.Type = PropertyTypeEnum.System_Array;
                metadata.IsEnumerable = true;
                return metadata;
            }

            // primitive:
            var propType = type.SpecialType switch
            {
                SpecialType.System_Enum => PropertyTypeEnum.System_Enum,
                SpecialType.System_Boolean => PropertyTypeEnum.System_Boolean,
                SpecialType.System_Char => PropertyTypeEnum.System_Char,
                SpecialType.System_SByte => PropertyTypeEnum.System_SByte,
                SpecialType.System_Byte => PropertyTypeEnum.System_Byte,
                SpecialType.System_Int16 => PropertyTypeEnum.System_Int16,
                SpecialType.System_UInt16 => PropertyTypeEnum.System_UInt16,
                SpecialType.System_Int32 => PropertyTypeEnum.System_Int32,
                SpecialType.System_UInt32 => PropertyTypeEnum.System_UInt32,
                SpecialType.System_Int64 => PropertyTypeEnum.System_Int64,
                SpecialType.System_UInt64 => PropertyTypeEnum.System_UInt64,
                SpecialType.System_Decimal => PropertyTypeEnum.System_Decimal,
                SpecialType.System_Single => PropertyTypeEnum.System_Single,
                SpecialType.System_Double => PropertyTypeEnum.System_Double,
                SpecialType.System_String => PropertyTypeEnum.System_String,
                SpecialType.System_IntPtr => PropertyTypeEnum.System_IntPtr,
                SpecialType.System_UIntPtr => PropertyTypeEnum.System_UIntPtr,
                SpecialType.System_DateTime => PropertyTypeEnum.System_DateTime,
                _ => PropertyTypeEnum.Other
            };

            if (propType != PropertyTypeEnum.Other)
            {
                metadata.IsPrimitive = true;
                metadata.Type = propType;
                return metadata;
            }

            //Generics:

            if (type.IsGeneric())
            {
                var typeFullName = type.GetFullName();
                propType = typeFullName switch
                {
                    "System.Collections.Generic.List" => PropertyTypeEnum.System_Collections_Generic_List_T,
                    "System.Collections.Generic.IList" => PropertyTypeEnum.System_Collections_Generic_IList_T,
                    "System.Collections.Generic.IEnumerable" => PropertyTypeEnum
                        .System_Collections_Generic_IEnumerable_T,
                    "System.Collections.Generic.ICollection" => PropertyTypeEnum
                        .System_Collections_Generic_ICollection_T,
                    _ => PropertyTypeEnum.Other
                };

                metadata.Type = propType;

                //Generic Enumerable
                if (propType != PropertyTypeEnum.Other)
                {
                    metadata.IsEnumerable = true;
                }

                //Generic class
                metadata.IsGenericClass = true;

                return metadata;
            }

            //class:
            if (type.SpecialType == SpecialType.None && type.TypeKind == TypeKind.Class)
            {
                metadata.IsNonGenericClass = true;
            }

            //unknown:
            return metadata;
        }

        protected class PropertyTypeMetadata
        {
            public string FullName { get; set; }
            public PropertyTypeEnum Type { get; set; }

            public bool IsGenericClass { get; set; }
            public bool IsPrimitive { get; set; }
            public bool IsEnumerable { get; set; }
            public bool IsNonGenericClass { get; set; }
        }
    }
}