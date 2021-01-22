using System;
using System.Collections.Generic;
using System.Linq;
using FastProjector.MapGenerator.Proccessing.Models;
using Microsoft.CodeAnalysis;

namespace FastProjector.MapGenerator.Proccessing
{
    public class PropertyCasting
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
        
        public Func<string, string> CastType(IPropertySymbol sourceProp, IPropertySymbol destinationProp)
        {
            return CastType(CreateCastMetadata(sourceProp), CreateCastMetadata(destinationProp));
        }

        public Func<string, string> CastType(CastMetaData sourceProp, IPropertySymbol destinationProp)
        {

            return CastType(sourceProp, CreateCastMetadata(destinationProp));


        }

        public Func<string, string> CastType(IPropertySymbol sourceProp, CastMetaData destinationProp)
        {
            return CastType(CreateCastMetadata(sourceProp), destinationProp);
        }

        public Func<string, string> CastType(CastMetaData sourceProp, CastMetaData destinationProp)
        {
               //might be castable
            

            if(sourceProp.Type != SpecialType.None)
            {
                var destinationSpecialType = destinationProp.Type;
                if(destinationSpecialType == SpecialType.None)
                {
                   return null;
                }

                var availableCast = _availableCasts[destinationSpecialType];

                if(availableCast != null)
                {
                    var castingDict = availableCast[sourceProp.Type];
                    if(castingDict != null)
                    {
                        return castingDict;
                    }
                }
            }
            
            return null;
        }


        private CastMetaData CreateCastMetadata(IPropertySymbol prop)
        {
            //if array:
            if(prop.Type is IArrayTypeSymbol arrayType)
            {
                return new CastMetaData 
                {
                    Type = SpecialType.System_Array,
                    HasGenericType = true,
                    GenericTypes = new GenericTypeMetaData[] {
                       new GenericTypeMetaData {
                           FullName = arrayType.ElementType.GetFullNamespace() + "." + arrayType.ElementType.Name,
                           IsCustomClass = arrayType.ElementType.TypeKind == TypeKind.Class 
                                && arrayType.ElementType.SpecialType == SpecialType.None
                        }
                        
                    }
                };
            }
            else {
                GenericTypeMetaData[] genericTypes = null;
                SpecialType propType = prop.Type.SpecialType;
                //some times collection types dont get recognized
                if(propType == SpecialType.None)
                {        
                    switch(prop.Type.GetFullNamespace() + "." + prop.Name)
                    {
                        case "System.Collections.Generic.List": 
                            propType = SpecialType.System_Collections_Generic_IList_T;
                            break;
                            case "System.Collections.Generic.IList": 
                            propType = SpecialType.System_Collections_Generic_IList_T;
                            break;
                        case "System.Collections.Generic.IEnumerable":
                            propType = SpecialType.System_Collections_Generic_IEnumerable_T;
                            break;
                        case "System.Collections.Generic.ICollection":
                            propType = SpecialType.System_Collections_Generic_ICollection_T;
                            break;
                    }
                } 

                if(prop.Type is INamedTypeSymbol typeSymbol && typeSymbol.Arity > 0 && typeSymbol.TypeArguments.NotNullAny())
                {
                    genericTypes = typeSymbol.TypeArguments.Select(s => new GenericTypeMetaData {
                        FullName = s.GetFullNamespace() + "." + s.Name,
                        IsCustomClass = s.TypeKind == TypeKind.Class && s.SpecialType == SpecialType.None  
                    }).ToArray(); 
                }

                return new CastMetaData
                {
                    Type = prop.Type.SpecialType,
                    HasGenericType = genericTypes.NotNullAny(),
                    GenericTypes = genericTypes
                };
            }
        }

    }
}