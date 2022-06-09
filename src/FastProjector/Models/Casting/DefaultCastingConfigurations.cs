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
            foreach (var castingConfiguration in ToListCasts()) yield return castingConfiguration;
            yield return ArrayCasts();
        }

        private static CastingConfiguration StringCasts()
        {
            return CastConfigurationBuilder.CreateCast()
                .From(PropertyType.System_Boolean)
                .From(PropertyType.System_Byte)
                .From(PropertyType.System_Int16)
                .From(PropertyType.System_Int32)
                .From(PropertyType.System_Int64)
                .From(PropertyType.System_Single)
                .From(PropertyType.System_Double)
                .From(PropertyType.System_Decimal)
                .To(PropertyType.System_String)
                .With((string source) => source + ".ToString()");
        }

        private static CastingConfiguration ShortCasts()
        {
            return CastConfigurationBuilder.CreateCast()
                .From(PropertyType.System_Int16)
                .From(PropertyType.System_Byte)
                .To(PropertyType.System_Int16)
                .With((string source) => "(short)" + source);
        }

        private static CastingConfiguration IntCasts()
        {
            return CastConfigurationBuilder.CreateCast()
                .From(PropertyType.System_Int16)
                .From(PropertyType.System_Byte)
                .From(PropertyType.System_Int16)
                .To(PropertyType.System_Int32)
                .With((string source) => "(int)" + source);
        }

        private static CastingConfiguration LongCasts()
        {
            return CastConfigurationBuilder.CreateCast()
                .From(PropertyType.System_Int16)
                .From(PropertyType.System_Byte)
                .From(PropertyType.System_Int16)
                .From(PropertyType.System_Int32)
                .To(PropertyType.System_Int64)
                .With((string source) => "(long)" + source);
        }

        private static IEnumerable<CastingConfiguration> ToListCasts()
        {
            return CastConfigurationBuilder.CreateCast()
                .From(PropertyType.System_Array)
                .From(PropertyType.System_Collections_Generic_IList_T)
                .From(PropertyType.System_Collections_Generic_List_T)
                .From(PropertyType.System_Collections_Generic_ICollection_T)
                .From(PropertyType.System_Collections_Generic_IEnumerable_T)
                .From(PropertyType.System_Collections_Generic_HashSet_T)
                .To(PropertyType.System_Collections_Generic_IEnumerable_T)
                .To(PropertyType.System_Collections_Generic_IList_T)
                .To(PropertyType.System_Collections_Generic_List_T)
                .To(PropertyType.System_Collections_Generic_ICollection_T)
                .To(PropertyType.System_Collections_Generic_HashSet_T)
                .With((string source) => source + ".ToList()");
        } 
        
        private static CastingConfiguration ArrayCasts()
        {
            return CastConfigurationBuilder.CreateCast()
                .From(PropertyType.System_Array)
                .From(PropertyType.System_Collections_Generic_IList_T)
                .From(PropertyType.System_Collections_Generic_List_T)
                .From(PropertyType.System_Collections_Generic_ICollection_T)
                .From(PropertyType.System_Collections_Generic_IEnumerable_T)
                .From(PropertyType.System_Collections_Generic_HashSet_T)
                .To(PropertyType.System_Array)
                .With((string source) => source + ".ToArray()");
        }
    }
}