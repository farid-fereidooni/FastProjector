using System;
using System.Collections.Generic;
using FastProjector.Helpers;
using Microsoft.CodeAnalysis;

namespace FastProjector.Models.TypeInformations
{
    internal abstract class TypeInformation : BaseTypeInformation
    {
        protected TypeInformation(ITypeSymbol type)
            : base(type)
        { }
        
        protected TypeInformation(string fullname, IEnumerable<TypeInformation> genericTypes)
            : base(fullname, genericTypes)
        { }
        
        public abstract PropertyType Type { get; }
        
        public virtual bool HasSameCategory(TypeInformation typeInformation)
        {
            return GetType() == typeInformation.GetType();
        }
        
        public static TypeInformation Create(IPropertySymbol propertySymbol)
        {
            return Create(propertySymbol?.Type);
        }
        public static TypeInformation Create(ITypeSymbol typeSymbol)
        {
            if(typeSymbol == null) throw new ArgumentNullException(nameof(typeSymbol));

            //if array:
            if (typeSymbol is IArrayTypeSymbol)
            {
                return new ArrayTypeInformation(typeSymbol);
            }

            // primitive:
            var primitiveType = GetPrimitiveType(typeSymbol);

            if (primitiveType != PropertyType.Other)
            {
                return new PrimitiveTypeInformation(typeSymbol, primitiveType);
            }

            //Generics:

            if (typeSymbol.IsGeneric())
            {
                var genericType = GetGenericCollectionType(typeSymbol.GetFullName());

                //Generic Enumerable
                if (genericType != PropertyType.Other)
                {
                    return new GenericCollectionTypeInformation(typeSymbol, genericType);
                }

                //Generic class
                return new GenericClassTypeInformation(typeSymbol);
            }

            //class:
            if (typeSymbol.SpecialType == SpecialType.None && typeSymbol.TypeKind == TypeKind.Class)
            {
                return new ClassTypeInformation(typeSymbol);
            }

            //unknown:
            return null;
        }
        
        private static PropertyType GetPrimitiveType(ITypeSymbol typeSymbol) =>
            typeSymbol.SpecialType switch
            {
                SpecialType.System_Enum => PropertyType.System_Enum,
                SpecialType.System_Boolean => PropertyType.System_Boolean,
                SpecialType.System_Char => PropertyType.System_Char,
                SpecialType.System_SByte => PropertyType.System_SByte,
                SpecialType.System_Byte => PropertyType.System_Byte,
                SpecialType.System_Int16 => PropertyType.System_Int16,
                SpecialType.System_UInt16 => PropertyType.System_UInt16,
                SpecialType.System_Int32 => PropertyType.System_Int32,
                SpecialType.System_UInt32 => PropertyType.System_UInt32,
                SpecialType.System_Int64 => PropertyType.System_Int64,
                SpecialType.System_UInt64 => PropertyType.System_UInt64,
                SpecialType.System_Decimal => PropertyType.System_Decimal,
                SpecialType.System_Single => PropertyType.System_Single,
                SpecialType.System_Double => PropertyType.System_Double,
                SpecialType.System_String => PropertyType.System_String,
                SpecialType.System_IntPtr => PropertyType.System_IntPtr,
                SpecialType.System_UIntPtr => PropertyType.System_UIntPtr,
                SpecialType.System_DateTime => PropertyType.System_DateTime,
                _ => PropertyType.Other
            };
        
        
    private static PropertyType GetGenericCollectionType(string typeFullName) =>
        typeFullName switch
        {
            "System.Collections.Generic.List" => PropertyType.System_Collections_Generic_List_T,
            "System.Collections.Generic.IList" => PropertyType.System_Collections_Generic_IList_T,
            "System.Collections.Generic.IEnumerable" => PropertyType
                .System_Collections_Generic_IEnumerable_T,
            "System.Collections.Generic.ICollection" => PropertyType
                .System_Collections_Generic_ICollection_T,
            "System.Collections.Generic.HashSet" => PropertyType.System_Collections_Generic_HashSet_T,
            _ => PropertyType.Other
        };
        

        
    }
}