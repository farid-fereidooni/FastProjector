using System;
using System.Collections.Generic;
using System.Linq;
using FastProjector.MapGenerator.Proccessing.Models;
using Microsoft.CodeAnalysis;

namespace FastProjector.MapGenerator.Proccessing
{
    internal class PropertyCasting
    {
        private Dictionary<PropertyTypeEnum, Dictionary<PropertyTypeEnum, Func<string, string>>> _availableCasts;
        public PropertyCasting()
        {
            _availableCasts = new Dictionary<PropertyTypeEnum, Dictionary<PropertyTypeEnum, Func<string, string>>>();
            InitializeCasts();
        }

        private void InitializeCasts()
        {
            static string toStringFunc(string source) => source + ".ToString()";

            #region string
            var stringCastableDict = new Dictionary<PropertyTypeEnum, Func<string, string>>();
            _availableCasts.Add(PropertyTypeEnum.System_String, stringCastableDict);

            stringCastableDict.Add(PropertyTypeEnum.System_Boolean, toStringFunc);
            stringCastableDict.Add(PropertyTypeEnum.System_Byte, toStringFunc);
            stringCastableDict.Add(PropertyTypeEnum.System_Int16, toStringFunc);
            stringCastableDict.Add(PropertyTypeEnum.System_Int32, toStringFunc);
            stringCastableDict.Add(PropertyTypeEnum.System_Int64, toStringFunc);
            stringCastableDict.Add(PropertyTypeEnum.System_Single, toStringFunc);
            stringCastableDict.Add(PropertyTypeEnum.System_Double, toStringFunc);
            stringCastableDict.Add(PropertyTypeEnum.System_Decimal, toStringFunc);


            #endregion

            #region short

            var shortCastableDict = new Dictionary<PropertyTypeEnum, Func<string, string>>();
            static string castToShort(string source) => "(short)" + source;

            _availableCasts.Add(PropertyTypeEnum.System_Int16, shortCastableDict);

            shortCastableDict.Add(PropertyTypeEnum.System_Byte, castToShort);

            #endregion

            #region int

            var intCastableDict = new Dictionary<PropertyTypeEnum, Func<string, string>>();
            static string castToInt(string source) => "(int)" + source;

            _availableCasts.Add(PropertyTypeEnum.System_Int32, intCastableDict);

            intCastableDict.Add(PropertyTypeEnum.System_Byte, castToInt);
            intCastableDict.Add(PropertyTypeEnum.System_Int16, castToInt);

            #endregion



            #region long

            var longCastableDict = new Dictionary<PropertyTypeEnum, Func<string, string>>();
            static string castToLong(string source) => "(long)" + source;

            _availableCasts.Add(PropertyTypeEnum.System_Int32, longCastableDict);

            longCastableDict.Add(PropertyTypeEnum.System_Byte, castToLong);
            longCastableDict.Add(PropertyTypeEnum.System_Int16, castToLong);
            longCastableDict.Add(PropertyTypeEnum.System_Int32, castToLong);

            #endregion

            #region IEnumerable
            var iEnumerableCastableDict = new Dictionary<PropertyTypeEnum, Func<string, string>>();
            static string castToIEnumerable(string source) => source;

            _availableCasts.Add(PropertyTypeEnum.System_Collections_Generic_IEnumerable_T, iEnumerableCastableDict);

            iEnumerableCastableDict.Add(PropertyTypeEnum.System_Array, castToIEnumerable);
            iEnumerableCastableDict.Add(PropertyTypeEnum.System_Collections_Generic_IList_T, castToIEnumerable);
            iEnumerableCastableDict.Add(PropertyTypeEnum.System_Collections_Generic_ICollection_T, castToIEnumerable);

            #endregion


            #region IList
            var iListCastableDict = new Dictionary<PropertyTypeEnum, Func<string, string>>();
            static string castToIList(string source) => source + ".ToList()";

            _availableCasts.Add(PropertyTypeEnum.System_Collections_Generic_IList_T, iListCastableDict);

            iListCastableDict.Add(PropertyTypeEnum.System_Collections_Generic_IEnumerable_T, castToIList);

            #endregion

        }

        public PropertyCastResult CastType(IPropertySymbol sourceProp, IPropertySymbol destinationProp)
        {
            return CastType(CreateCastMetadata(sourceProp), CreateCastMetadata(destinationProp));
        }

        public PropertyCastResult CastType(CastMetaData sourceProp, IPropertySymbol destinationProp)
        {

            return CastType(sourceProp, CreateCastMetadata(destinationProp));


        }

        public PropertyCastResult CastType(IPropertySymbol sourceProp, CastMetaData destinationProp)
        {
            return CastType(CreateCastMetadata(sourceProp), destinationProp);
        }

        public PropertyCastResult CastType(CastMetaData sourceProp, CastMetaData destinationProp)
        {
            var result = new PropertyCastResult()
            {
                SourcePropertyCastMetaData = sourceProp,
                DestinationPropertyCastMetaData = destinationProp,
            };

            //same types, might be castable
            if (sourceProp.TypeCategory == destinationProp.TypeCategory)
            {
                //collections:
                if (sourceProp.TypeCategory == PropertyTypeCategoryEnum.CollectionPrimitive ||
                   sourceProp.TypeCategory == PropertyTypeCategoryEnum.CollectionObject)
                {
                    if (!IsSameGenericTypes(sourceProp.GenericTypes, destinationProp.GenericTypes))
                    {
                        result.IsUnMapable = true;
                        result.Cast = null;
                        return result;
                    }
                }

                var availableCast = _availableCasts[destinationProp.Type];

                if (availableCast != null)
                {
                    var castingFunc = availableCast[sourceProp.Type];
                    if (castingFunc != null)
                    {
                        result.IsUnMapable = false;
                        result.Cast = castingFunc;
                        return result;
                    }
                }

            }         
            result.IsUnMapable = true;
            result.Cast = null;
            return result;
        }


        private CastMetaData CreateCastMetadata(IPropertySymbol prop)
        {

            var propTypeInfo = DeterminePropertyType(prop.Type);
            //Enumerables:
            if(propTypeInfo.IsGenericEnumerable)
            {
                PropertyTypeInfo  enumerableArgumentTypeInfo = null;
                
                //array:
                if(propTypeInfo.Type == PropertyTypeEnum.System_Array)
                {
                    enumerableArgumentTypeInfo = DeterminePropertyType(((IArrayTypeSymbol)prop.Type).ElementType);
                }
                //other enumerable types:
                else
                {
                    enumerableArgumentTypeInfo = DeterminePropertyType((prop.Type as INamedTypeSymbol).TypeArguments.FirstOrDefault());
                }
                
                return new CastMetaData
                {
                    FullName = propTypeInfo.FullName,
                    HasGenericType = true,
                    Type = propTypeInfo.Type,
                    TypeCategory = enumerableArgumentTypeInfo.IsPrimitive ? PropertyTypeCategoryEnum.CollectionPrimitive : PropertyTypeCategoryEnum.CollectionObject,
                    GenericTypes = new GenericMetaData[] {
                        new GenericMetaData {
                            FullName = enumerableArgumentTypeInfo.FullName,
                            IsPrimitive = enumerableArgumentTypeInfo.IsPrimitive
                        }
                    }
                };
            }
            //genericClass:
            if(propTypeInfo.IsGenericClass)
            {
                return new CastMetaData
                {
                    FullName = propTypeInfo.FullName,
                    HasGenericType = true,
                    Type = propTypeInfo.Type,
                    TypeCategory = PropertyTypeCategoryEnum.SingleGenericClass,
                    GenericTypes = ((INamedTypeSymbol)prop.Type).TypeArguments.Select(typeArg => 
                        {
                            var typeArgTypeInfo = DeterminePropertyType(typeArg);
                            return new GenericMetaData
                            {
                                FullName = typeArgTypeInfo.FullName,
                                IsPrimitive = typeArgTypeInfo.IsPrimitive
                            };
                        }
                    ).ToArray()
                };
            }

            //primitive:
            if(propTypeInfo.IsPrimitive)
            {
                return new CastMetaData
                {
                    FullName = propTypeInfo.FullName,
                    HasGenericType = false,
                    GenericTypes = null,
                    Type = propTypeInfo.Type,
                    TypeCategory = PropertyTypeCategoryEnum.SinglePrimitive
                };
            }

            //single class or unknown
            return new CastMetaData
            {
                FullName = propTypeInfo.FullName,
                HasGenericType = false,
                GenericTypes = null,
                Type = propTypeInfo.Type,
                TypeCategory = propTypeInfo.IsNonGenericClass ? PropertyTypeCategoryEnum.SingleNonGenenericClass : PropertyTypeCategoryEnum.Unknown
            };
        }

        private PropertyTypeInfo DeterminePropertyType(ITypeSymbol type)
        {
            //if array:
            if (type is IArrayTypeSymbol)
            {
            
                return new PropertyTypeInfo
                {
                    FullName = type.GetFullName(),
                    Type = PropertyTypeEnum.System_Array,
                    IsGenericEnumerable = true
                };
            }
            else
            {
                // primitive:
                PropertyTypeEnum propType = type.SpecialType switch
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
                    return new PropertyTypeInfo
                    {
                         FullName = type.GetFullName(),
                         IsPrimitive = true,
                         Type = propType
                    };
                }

                //Generics:

                if (type is INamedTypeSymbol typeSymbol && typeSymbol.Arity > 0 && typeSymbol.TypeArguments.NotNullAny())
                {

                    var typeFullName = type.GetFullName();
                    propType = typeFullName switch {
                        
                        "System.Collections.Generic.List" => PropertyTypeEnum.System_Collections_Generic_List_T,
                        "System.Collections.Generic.IList" => PropertyTypeEnum.System_Collections_Generic_IList_T,
                        "System.Collections.Generic.IEnumerable" =>  PropertyTypeEnum.System_Collections_Generic_IEnumerable_T,
                        "System.Collections.Generic.ICollection" => PropertyTypeEnum.System_Collections_Generic_ICollection_T,
                        _ => PropertyTypeEnum.Other
                    };
                    //Generic Enumerable

                    if(propType != PropertyTypeEnum.Other)
                    {
                        return new PropertyTypeInfo
                        {
                            FullName = typeFullName,
                            IsGenericEnumerable = true,
                            Type = propType
                        };
                    }
                
                    //Generic class:

                    return new PropertyTypeInfo
                    {
                        FullName = typeFullName,
                        IsGenericClass = true,
                        Type = propType
                    };

                }
                else 
                {
                    //class:
                    if(type.SpecialType == SpecialType.None && type.TypeKind == TypeKind.Class)                    
                    {
                        return new PropertyTypeInfo
                        {
                            FullName = type.GetFullName(),
                            IsNonGenericClass = true,
                            Type = propType
                        };                        
                    } 
                    //unknown:
                    else {
                        return new PropertyTypeInfo
                        {
                            FullName = type.GetFullName(),
                            Type = propType
                        };      
                    }
                }
            }
        }

        private bool IsSameGenericTypes(GenericMetaData[] sourceGenericTypes, GenericMetaData[] destinationGenericTypes)
        {
            if (sourceGenericTypes == null && destinationGenericTypes == null)
                return true;
            if (sourceGenericTypes != null && destinationGenericTypes != null &&
               sourceGenericTypes.Length == destinationGenericTypes.Length)
            {
                for (int i = 0; i < sourceGenericTypes.Length; i++)
                {
                    if (sourceGenericTypes[i].FullName != destinationGenericTypes[i].FullName)
                        return false;
                }
                return true;
            }
            else
                return false;
        }

    }
}