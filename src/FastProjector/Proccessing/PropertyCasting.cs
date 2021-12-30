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
            static string CastToInt(string source) => "(int)" + source;

            _availableCasts.Add(PropertyTypeEnum.System_Int32, intCastableDict);

            intCastableDict.Add(PropertyTypeEnum.System_Byte, CastToInt);
            intCastableDict.Add(PropertyTypeEnum.System_Int16, CastToInt);

            #endregion

            #region long

            var longCastableDict = new Dictionary<PropertyTypeEnum, Func<string, string>>();
            static string CastToLong(string source) => "(long)" + source;

            _availableCasts.Add(PropertyTypeEnum.System_Int32, longCastableDict);

            longCastableDict.Add(PropertyTypeEnum.System_Byte, CastToLong);
            longCastableDict.Add(PropertyTypeEnum.System_Int16, CastToLong);
            longCastableDict.Add(PropertyTypeEnum.System_Int32, CastToLong);

            #endregion

            #region IEnumerable
            var iEnumerableCastableDict = new Dictionary<PropertyTypeEnum, Func<string, string>>();

            _availableCasts.Add(PropertyTypeEnum.System_Collections_Generic_IEnumerable_T, iEnumerableCastableDict);

            iEnumerableCastableDict.Add(PropertyTypeEnum.System_Array, source => source + ".ToArray()");
            iEnumerableCastableDict.Add(PropertyTypeEnum.System_Collections_Generic_IList_T, source => source + ".ToList()");
            iEnumerableCastableDict.Add(PropertyTypeEnum.System_Collections_Generic_ICollection_T, source => source + ".ToList()");
            iEnumerableCastableDict.Add(PropertyTypeEnum.System_Collections_Generic_IEnumerable_T, source => source + ".ToList()");

            #endregion

            #region IList
            var iListCastableDict = new Dictionary<PropertyTypeEnum, Func<string, string>>();
            static string CastToIList(string source) => source + ".ToList()";

            _availableCasts.Add(PropertyTypeEnum.System_Collections_Generic_IList_T, iListCastableDict);

            iListCastableDict.Add(PropertyTypeEnum.System_Collections_Generic_IEnumerable_T, CastToIList);
            iListCastableDict.Add(PropertyTypeEnum.System_Collections_Generic_ICollection_T, CastToIList);
            iListCastableDict.Add(PropertyTypeEnum.System_Collections_Generic_List_T, CastToIList);
            iListCastableDict.Add(PropertyTypeEnum.System_Array, CastToIList);

            #endregion

        }

        public PropertyCastResult CastType(IPropertySymbol sourceProp, IPropertySymbol destinationProp)
        {
            return CastType(new PropertyTypeInformation(sourceProp), new PropertyTypeInformation(destinationProp));
        }

        public PropertyCastResult CastType(PropertyTypeInformation sourceProp, IPropertySymbol destinationProp)
        {

            return CastType(sourceProp, new PropertyTypeInformation(destinationProp));


        }

        public PropertyCastResult CastType(IPropertySymbol sourceProp, PropertyTypeInformation destinationProp)
        {
            return CastType(new PropertyTypeInformation(sourceProp), destinationProp);
        }

        public PropertyCastResult CastType(PropertyTypeInformation sourceProp, PropertyTypeInformation destinationProp)
        {
            var result = new PropertyCastResult()
            {
                SourceProperyTypeInfo = sourceProp,
                DestinationProperyTypeInfo = destinationProp,
            };

            //same category types, might be castable
            if (sourceProp.TypeCategory == destinationProp.TypeCategory)
            {
                //collections:
                if (sourceProp.TypeCategory == PropertyTypeCategoryEnum.CollectionPrimitive ||
                   sourceProp.TypeCategory == PropertyTypeCategoryEnum.CollectionObject)
                {
                    if (!sourceProp.HasSameCollectionType(destinationProp))
                    {
                        result.IsUnMapable = true;
                        result.Cast = null;
                        return result;
                    }
                }

                var availableCast = _availableCasts[destinationProp.Type];

                var castingFunc = availableCast?[sourceProp.Type];
                
                if (castingFunc != null)
                {
                    result.IsUnMapable = false;
                    result.Cast = castingFunc;
                    return result;
                }

            }
            result.IsUnMapable = true;
            result.Cast = null;
            return result;
        }

    }
}