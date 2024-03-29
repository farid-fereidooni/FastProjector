using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Claims;
using System.Text;
using FastProjector.Contracts;
using FastProjector.Helpers;
using FastProjector.Models;
using FastProjector.Models.TypeInformations;

namespace FastProjector.Repositories
{
    internal class MapRepository : IMapRepository
    {
        private readonly Dictionary<string, Dictionary<string, ModelMap>> _cachedMapMetaData;

        public MapRepository()
        {
            _cachedMapMetaData = new Dictionary<string, Dictionary<string, ModelMap>>();
        }

        public void Add(ModelMap map)
        {
            var sourceTypeKey = GenerateKey(map.SourceType);
            var destinationTypeKey = GenerateKey(map.DestinationType);

            var existence = Exists(sourceTypeKey, destinationTypeKey);
            if (existence.sourceExistence && existence.destinationExistence)
                return;

            if (existence.sourceExistence)
            {
                _cachedMapMetaData[sourceTypeKey].Add(destinationTypeKey, map);
            }

            _cachedMapMetaData[sourceTypeKey] = new Dictionary<string, ModelMap>
            {
                {destinationTypeKey, map}
            };
        }

        public ModelMap Get(TypeInformation sourceType, TypeInformation destinationType)
        {
            return Get(
                GenerateKey(sourceType),
                GenerateKey(destinationType)
            );
        }


        public IReadOnlyDictionary<string, IReadOnlyCollection<ModelMap>> GetAll()
        {
            return _cachedMapMetaData.ToDictionary(k => k.Key,
                v => (IReadOnlyCollection<ModelMap>) v.Value.Values.ToList());
        }

        private ModelMap Get(string sourceTypeKey, string destinationTypeKey)
        {
            if (!_cachedMapMetaData.TryGetValue(sourceTypeKey, out var destinationTypes)) return null;

            return destinationTypes.TryGetValue(destinationTypeKey, out var metaData) ? metaData : null;
        }


        public bool Exists(TypeInformation sourceType, TypeInformation destinationType)
        {
            return Exists(
                GenerateKey(sourceType),
                GenerateKey(destinationType)
            ) == (true, true);
        }

        private (bool sourceExistence, bool destinationExistence) Exists(string sourceTypeKey,
            string destinationTypeKey)
        {
            (bool sourceExistence, bool destinationExistence) result = (false, false);

            if (_cachedMapMetaData.TryGetValue(sourceTypeKey, out var destinationTypes))
            {
                result.sourceExistence = true;
                result.destinationExistence = destinationTypes.ContainsKey(destinationTypeKey);
            }

            return result;
        }

        private static string GenerateKey(TypeInformation propType)
        {
            return propType.FullName;
        }
    }
}