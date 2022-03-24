using System.Collections.Generic;
using System.Linq;
using System.Text;
using FastProjector.Contracts;
using FastProjector.Helpers;
using FastProjector.Models;
using SourceCreationHelper.Interfaces;

namespace FastProjector.Services
{
    internal class MapCache: IMapCache
    {
        private readonly Dictionary<string, Dictionary<string, ISourceText>> _cachedMapMetaData;

        public MapCache()
        {
            _cachedMapMetaData = new Dictionary<string, Dictionary<string, ISourceText>>();
        }

        public void Add(TypeInformation sourceType, TypeInformation destinationType, ISourceText sourceText)
        {
            var sourceTypeKey = GenerateKey(sourceType);
            var destinationTypeKey = GenerateKey(destinationType);

            var existence = Exists(sourceTypeKey, destinationTypeKey);
            if (existence.sourceExistence && existence.destinationExistence)
                return;

            if(existence.sourceExistence)
            {
                _cachedMapMetaData[sourceTypeKey].Add(destinationTypeKey, sourceText);
            }

            _cachedMapMetaData[sourceTypeKey] = new Dictionary<string, ISourceText>
            {
                {destinationTypeKey, sourceText}
            };
        }

        public ISourceText Get(TypeInformation sourceType, TypeInformation destinationType)
        {
            return Get(
                GenerateKey(sourceType),
                GenerateKey(destinationType)
            );
        }

        private ISourceText Get(string sourceTypeKey, string destinationTypeKey)
        {
            if (!_cachedMapMetaData.TryGetValue(sourceTypeKey, out var destinationTypes)) return null;
            
            return destinationTypes.TryGetValue(destinationTypeKey, out var source) ? source : null;
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
            
           if(_cachedMapMetaData.TryGetValue(sourceTypeKey, out var destinationTypes))
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