using System;
using FastProjector.Helpers;
using Microsoft.CodeAnalysis;

namespace FastProjector.Models.PropertyTypeInformations
{
    internal abstract class PropertyTypeInformation : TypeInformation
    {

        public PropertyTypeInformation(ITypeSymbol type)
            : base(type)
        { }
        
        public abstract PropertyTypeEnum Type { get; }
        
        public static PropertyTypeInformation CreatePropertyTypeInformation(IPropertySymbol propertySymbol)
        {
            return CreatePropertyTypeInformation(propertySymbol?.Type);
        }
        public static PropertyTypeInformation CreatePropertyTypeInformation(ITypeSymbol typeSymbol)
        {
            if(typeSymbol == null) throw new ArgumentNullException(nameof(typeSymbol));

            //if array:
            if (typeSymbol is IArrayTypeSymbol)
            {
                return new ArrayPropertyTypeInformation(typeSymbol);
            }

            // primitive:
            var primitiveType = GetPrimitiveType(typeSymbol);

            if (primitiveType != PropertyTypeEnum.Other)
            {
                return new PrimitivePropertyTypeInformation(typeSymbol, primitiveType);
            }

            //Generics:

            if (typeSymbol.IsGeneric())
            {
                var genericType = GetGenericCollectionType(typeSymbol.GetFullName());

                //Generic Enumerable
                if (genericType != PropertyTypeEnum.Other)
                {
                    return new GenericCollectionPropertyTypeInformation(typeSymbol, genericType);
                }

                //Generic class
                return new GenericClassPropertyTypeInformation(typeSymbol);
            }

            //class:
            if (typeSymbol.SpecialType == SpecialType.None && typeSymbol.TypeKind == TypeKind.Class)
            {
                return new ClassPropertyTypeInformation(typeSymbol);
            }

            //unknown:
            return null;
        }
        
 

        private static PropertyTypeEnum GetPrimitiveType(ITypeSymbol typeSymbol) =>
            typeSymbol.SpecialType switch
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
        
        
    private static PropertyTypeEnum GetGenericCollectionType(string typeFullName) =>
        typeFullName switch
        {
            "System.Collections.Generic.List" => PropertyTypeEnum.System_Collections_Generic_List_T,
            "System.Collections.Generic.IList" => PropertyTypeEnum.System_Collections_Generic_IList_T,
            "System.Collections.Generic.IEnumerable" => PropertyTypeEnum
                .System_Collections_Generic_IEnumerable_T,
            "System.Collections.Generic.ICollection" => PropertyTypeEnum
                .System_Collections_Generic_ICollection_T,
            _ => PropertyTypeEnum.Other
        };
        

        
    }
}