using System.Collections.Generic;
using System.Linq;
using System.Text;
using FastProjector.MapGenerator.Proccessing.Contracts;
using FastProjector.MapGenerator.Proccessing.Models;

namespace FastProjector.MapGenerator.Proccessing
{
    internal class MapCache: IMapCache
    {
        private readonly Dictionary<string, Dictionary<string, ModelMapMetaData>> cachedMapMetaData;

        public MapCache()
        {
            cachedMapMetaData = new Dictionary<string, Dictionary<string, ModelMapMetaData>>();
        }

        public void Add(ModelMapMetaData mapMetaData)
        {
            var sourceTypeKey = GenerateKey(mapMetaData.SourceType);
            var destinationTypeKey = GenerateKey(mapMetaData.DestinationType);

            var existence = Exists(sourceTypeKey, destinationTypeKey);
            if (existence.sourceExistence && existence.destinationExistence)
                return;

            if(existence.sourceExistence)
            {
                cachedMapMetaData[sourceTypeKey].Add(destinationTypeKey, mapMetaData);
            }

            cachedMapMetaData[sourceTypeKey] = new Dictionary<string, ModelMapMetaData>
            {
                {destinationTypeKey, mapMetaData}
            };
        }

        public ModelMapMetaData Get(TypeInformation sourceType, TypeInformation destinationType)
        {
            return Get(
                GenerateKey(sourceType),
                GenerateKey(destinationType)
            );
        }

        private ModelMapMetaData Get(string sourceTypeKey, string destinationTypeKey)
        {
            if (!cachedMapMetaData.TryGetValue(sourceTypeKey, out var destinationTypes)) return null;
            
            return destinationTypes.TryGetValue(destinationTypeKey, out var metaData) ? metaData : null;
        }
        

        public bool Exists(TypeInformation sourceType, TypeInformation destinationType)
        {
            return Exists(
                GenerateKey(sourceType),
                GenerateKey(destinationType)
            ) == (true, true);
        }
        
        private (bool sourceExistence, bool destinationExistence) Exists(string sourceTypeKey, string destinationTypeKey)
        {
            (bool sourceExistence, bool destinationExistence) result = (false, false);
            
           if(cachedMapMetaData.TryGetValue(sourceTypeKey, out var destinationTypes))
           {
               result.sourceExistence = true;
               result.destinationExistence = destinationTypes.ContainsKey(destinationTypeKey);
           }

           return result;
        }

        private static string GenerateKey(TypeInformation propType)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append(propType.FullName);
            if (propType.HasGenericTypes())
            {
                stringBuilder.Append("<");
                string.Join(",", propType.GenericTypes.Select(s => s.FullName));
                stringBuilder.Append(">");
            }

            return stringBuilder.ToString();
        }

    }
}