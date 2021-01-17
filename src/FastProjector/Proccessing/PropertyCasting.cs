using System;
using System.Collections.Generic;

namespace FastProjector.MapGenerator.Proccessing
{
    public class PropertyCasting
    {
        private Dictionary<Type, Dictionary<Type, Func<string, string>>> _availableCasts;
        public PropertyCasting()
        {
            _availableCasts = new Dictionary<Type, Dictionary<Type, Func<string, string>>>();
            InitializeCasts();
        }

        private void InitializeCasts()
        {
            static string toStringFunc(string source) => source + ".ToString()";

            #region string
            var stringCastableDict = new Dictionary<Type, Func<string, string>>();
            _availableCasts.Add(typeof(string), stringCastableDict);

            stringCastableDict.Add(typeof(bool), toStringFunc);
            stringCastableDict.Add(typeof(byte), toStringFunc);
            stringCastableDict.Add(typeof(short), toStringFunc);
            stringCastableDict.Add(typeof(int), toStringFunc);
            stringCastableDict.Add(typeof(long), toStringFunc);
            stringCastableDict.Add(typeof(float), toStringFunc);
            stringCastableDict.Add(typeof(double), toStringFunc);
            stringCastableDict.Add(typeof(decimal), toStringFunc);


            #endregion

            #region short

            var shortCastableDict = new Dictionary<Type, Func<string, string>>();
            static string castToShort(string source) => "(short)" + source;

            _availableCasts.Add(typeof(int), shortCastableDict);

            shortCastableDict.Add(typeof(byte), castToShort);
            
            #endregion

            #region int

            var intCastableDict = new Dictionary<Type, Func<string, string>>();
            static string castToInt(string source) => "(int)" + source;

            _availableCasts.Add(typeof(int), intCastableDict);

            intCastableDict.Add(typeof(byte), castToInt);
            intCastableDict.Add(typeof(short), castToInt);

            #endregion



            #region long

            var longCastableDict = new Dictionary<Type, Func<string, string>>();
            static string castToLong(string source) => "(long)" + source;

            _availableCasts.Add(typeof(int), longCastableDict);

            longCastableDict.Add(typeof(byte), castToLong);
            longCastableDict.Add(typeof(short), castToLong);
            longCastableDict.Add(typeof(int), castToLong);
            
            #endregion

        }


    }
}