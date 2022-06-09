using System.Collections;
using System.Collections.Generic;
using FastProjector.Helpers;
using FastProjector.Models.TypeInformations;

namespace FastProjector.Models.Casting
{
    internal static class DefaultCastingConfigurations
    {
        public static IEnumerable<CastingConfiguration> GetConfigurations()
        {
            yield return StringCasts();
            yield return ShortCasts();
            yield return IntCasts();
            yield return LongCasts();
            yield return IEnumerableCasts();
            yield return IListCasts();
            yield return ListCasts();
        }

        private static CastingConfiguration StringCasts()
        {
            return CastConfigurationBuilder.CreateCast()
                .From(PropertyTypeEnum.System_Boolean)
                .From(PropertyTypeEnum.System_Byte)
                .From(PropertyTypeEnum.System_Int16)
                .From(PropertyTypeEnum.System_Int32)
                .From(PropertyTypeEnum.System_Int64)
                .From(PropertyTypeEnum.System_Single)
                .From(PropertyTypeEnum.System_Double)
                .From(PropertyTypeEnum.System_Decimal)
                .To(PropertyTypeEnum.System_String)
                .With((string source) => source + ".ToString()");
        }

        private static CastingConfiguration ShortCasts()
        {
            return CastConfigurationBuilder.CreateCast()
                .From(PropertyTypeEnum.System_Int16)
                .From(PropertyTypeEnum.System_Byte)
                .To(PropertyTypeEnum.System_Int16)
                .With((string source) => "(short)" + source);
        }

        private static CastingConfiguration IntCasts()
        {
            return CastConfigurationBuilder.CreateCast()
                .From(PropertyTypeEnum.System_Int16)
                .From(PropertyTypeEnum.System_Byte)
                .From(PropertyTypeEnum.System_Int16)
                .To(PropertyTypeEnum.System_Int32)
                .With((string source) => "(int)" + source);
        }

        private static CastingConfiguration LongCasts()
        {
            return CastConfigurationBuilder.CreateCast()
                .From(PropertyTypeEnum.System_Int16)
                .From(PropertyTypeEnum.System_Byte)
                .From(PropertyTypeEnum.System_Int16)
                .From(PropertyTypeEnum.System_Int32)
                .To(PropertyTypeEnum.System_Int64)
                .With((string source) => "(long)" + source);
        }

        private static CastingConfiguration IEnumerableCasts()
        {
            return CastConfigurationBuilder.CreateCast()
                .From(PropertyTypeEnum.System_Array)
                .From(PropertyTypeEnum.System_Collections_Generic_IList_T)
                .From(PropertyTypeEnum.System_Collections_Generic_List_T)
                .From(PropertyTypeEnum.System_Collections_Generic_ICollection_T)
                .From(PropertyTypeEnum.System_Collections_Generic_IEnumerable_T)
                .To(PropertyTypeEnum.System_Collections_Generic_IEnumerable_T)
                .With((string source) => source + ".ToList()");
        }

        private static CastingConfiguration IListCasts()
        {
            return CastConfigurationBuilder.CreateCast()
                .From(PropertyTypeEnum.System_Array)
                .From(PropertyTypeEnum.System_Collections_Generic_IList_T)
                .From(PropertyTypeEnum.System_Collections_Generic_List_T)
                .From(PropertyTypeEnum.System_Collections_Generic_ICollection_T)
                .From(PropertyTypeEnum.System_Collections_Generic_IEnumerable_T)
                .To(PropertyTypeEnum.System_Collections_Generic_IList_T)
                .With((string source) => source + ".ToList()");
        }
        
        private static CastingConfiguration ListCasts()
        {
            return CastConfigurationBuilder.CreateCast()
                .From(PropertyTypeEnum.System_Array)
                .From(PropertyTypeEnum.System_Collections_Generic_IList_T)
                .From(PropertyTypeEnum.System_Collections_Generic_List_T)
                .From(PropertyTypeEnum.System_Collections_Generic_ICollection_T)
                .From(PropertyTypeEnum.System_Collections_Generic_IEnumerable_T)
                .To(PropertyTypeEnum.System_Collections_Generic_List_T)
                .With((string source) => source + ".ToList()");
        }
    }
}