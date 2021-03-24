using System;
using System.Collections.Generic;
using System.Linq;
using FastProjector.MapGenerator.Proccessing.Models;
using Microsoft.CodeAnalysis;

namespace FastProjector.MapGenerator.Proccessing
{
    internal class PropertyCasting
    {
        private Dictionary<SpecialType, Dictionary<SpecialType, Func<string, string>>> _availableCasts;
        public PropertyCasting()
        {
            _availableCasts = new Dictionary<SpecialType, Dictionary<SpecialType, Func<string, string>>>();
            InitializeCasts();
        }

        private void InitializeCasts()
        {
            static string toStringFunc(string source) => source + ".ToString()";

            #region string
            var stringCastableDict = new Dictionary<SpecialType, Func<string, string>>();
            _availableCasts.Add(SpecialType.System_String, stringCastableDict);

            stringCastableDict.Add(SpecialType.System_Boolean, toStringFunc);
            stringCastableDict.Add(SpecialType.System_Byte, toStringFunc);
            stringCastableDict.Add(SpecialType.System_Int16, toStringFunc);
            stringCastableDict.Add(SpecialType.System_Int32, toStringFunc);
            stringCastableDict.Add(SpecialType.System_Int64, toStringFunc);
            stringCastableDict.Add(SpecialType.System_Single, toStringFunc);
            stringCastableDict.Add(SpecialType.System_Double, toStringFunc);
            stringCastableDict.Add(SpecialType.System_Decimal, toStringFunc);


            #endregion

            #region short

            var shortCastableDict = new Dictionary<SpecialType, Func<string, string>>();
            static string castToShort(string source) => "(short)" + source;

            _availableCasts.Add(SpecialType.System_Int16, shortCastableDict);

            shortCastableDict.Add(SpecialType.System_Byte, castToShort);

            #endregion

            #region int

            var intCastableDict = new Dictionary<SpecialType, Func<string, string>>();
            static string castToInt(string source) => "(int)" + source;

            _availableCasts.Add(SpecialType.System_Int32, intCastableDict);

            intCastableDict.Add(SpecialType.System_Byte, castToInt);
            intCastableDict.Add(SpecialType.System_Int16, castToInt);

            #endregion



            #region long

            var longCastableDict = new Dictionary<SpecialType, Func<string, string>>();
            static string castToLong(string source) => "(long)" + source;

            _availableCasts.Add(SpecialType.System_Int32, longCastableDict);

            longCastableDict.Add(SpecialType.System_Byte, castToLong);
            longCastableDict.Add(SpecialType.System_Int16, castToLong);
            longCastableDict.Add(SpecialType.System_Int32, castToLong);

            #endregion

            #region IEnumerable
            var iEnumerableCastableDict = new Dictionary<SpecialType, Func<string, string>>();
            static string castToIEnumerable(string source) => source;

            _availableCasts.Add(SpecialType.System_Collections_Generic_IEnumerable_T, iEnumerableCastableDict);

            iEnumerableCastableDict.Add(SpecialType.System_Array, castToIEnumerable);
            iEnumerableCastableDict.Add(SpecialType.System_Collections_Generic_IList_T, castToIEnumerable);
            iEnumerableCastableDict.Add(SpecialType.System_Collections_Generic_ICollection_T, castToIEnumerable);

            #endregion


            #region IList
            var iListCastableDict = new Dictionary<SpecialType, Func<string, string>>();
            static string castToIList(string source) => source + ".ToList()";

            _availableCasts.Add(SpecialType.System_Collections_Generic_IList_T, iListCastableDict);

            iListCastableDict.Add(SpecialType.System_Collections_Generic_IEnumerable_T, castToIList);

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

            if (sourceProp.Type == SpecialType.None || destinationProp.Type == SpecialType.None)
            {
                result.IsUnMapable = true;
                result.Cast = null;
                return result;
            }

            //same types, might be castable
            if (sourceProp.TypeCategory == destinationProp.TypeCategory)
            {
                //collections:
                if (sourceProp.TypeCategory == PropertyTypeCategoryEnum.CollectionPrimitive ||
                   sourceProp.TypeCategory == PropertyTypeCategoryEnum.CollectionObject)
                {
                    if (!IsSameGenericType(sourceProp.GenericTypes, destinationProp.GenericTypes))
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
            //if array:
            if (prop.Type is IArrayTypeSymbol arrayType)
            {
                return new CastMetaData
                {
                    Type = SpecialType.System_Array,
                    HasGenericType = true,
                    TypeCategory =  arrayType.ElementType.TypeKind == TypeKind.Class
                                    && arrayType.ElementType.SpecialType == SpecialType.None ? PropertyTypeCategoryEnum.CollectionObject : PropertyTypeCategoryEnum.CollectionPrimitive,         
                    GenericTypes = new CastMetaData[] {
                       new CastMetaData {
                           FullName = arrayType.ElementType.GetFullNamespace() + "." + arrayType.ElementType.Name,
                           TypeCategory = arrayType.ElementType.TypeKind == TypeKind.Class
                                && arrayType.ElementType.SpecialType == SpecialType.None ? PropertyTypeCategoryEnum.SingleObject : PropertyTypeCategoryEnum.SinglePrimitive
                        }

                    }
                };
            }
            else
            {
                SpecialType propType = prop.Type.SpecialType;
                bool isGenericCollection = false;
                //some times collection types dont get recognized
                if (propType == SpecialType.None)
                {
                    switch (prop.Type.GetFullNamespace() + "." + prop.Name)
                    {
                        case "System.Collections.Generic.List":
                            propType = SpecialType.System_Collections_Generic_IList_T;
                            isGenericCollection = true;
                            break;
                        case "System.Collections.Generic.IList":
                            propType = SpecialType.System_Collections_Generic_IList_T;
                            isGenericCollection = true;
                            break;
                        case "System.Collections.Generic.IEnumerable":
                            propType = SpecialType.System_Collections_Generic_IEnumerable_T;
                            isGenericCollection = true;
                            break;
                        case "System.Collections.Generic.ICollection":
                            propType = SpecialType.System_Collections_Generic_ICollection_T;
                            isGenericCollection = true;
                            break;  
                    }
                }
                
                var result = new CastMetaData
                {
                    Type = propType,                    
                };

                if (prop.Type is INamedTypeSymbol typeSymbol && typeSymbol.Arity > 0 && typeSymbol.TypeArguments.NotNullAny())
                {
                    result.HasGenericType = true;

                    result.GenericTypes = typeSymbol.TypeArguments.Select(s => new CastMetaData
                    {
                        FullName = s.GetFullNamespace() + "." + s.Name,
                        TypeCategory = s.TypeKind == TypeKind.Class && s.SpecialType == SpecialType.None ? PropertyTypeCategoryEnum.SingleObject : PropertyTypeCategoryEnum.SinglePrimitive
                    }).ToArray();
            
                    if(isGenericCollection)
                    {
                        if(result.GenericTypes.FirstOrDefault().TypeCategory == PropertyTypeCategoryEnum.SinglePrimitive)
                            result.TypeCategory = PropertyTypeCategoryEnum.CollectionPrimitive;
                        else
                            result.TypeCategory = PropertyTypeCategoryEnum.CollectionObject;
                    }
                    else 
                    {
                        result.TypeCategory = PropertyTypeCategoryEnum.GenericClass;
                    }
                }
                else 
                {
                    if(propType == SpecialType.None && prop.Type.TypeKind == TypeKind.Class)                    
                        result.TypeCategory = PropertyTypeCategoryEnum.SingleObject;
                }

              
                
            }
        }

        private bool IsSameGenericType(CastMetaData[] sourceGenericTypes, CastMetaData[] destinationGenericTypes)
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