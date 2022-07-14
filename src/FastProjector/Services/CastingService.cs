using System;
using System.Collections.Generic;
using FastProjector.Contracts;
using FastProjector.Helpers;
using FastProjector.Models;
using FastProjector.Models.Casting;
using FastProjector.Models.TypeInformations;

namespace FastProjector.Services
{
    internal class CastingService : ICastingService
    {
        private readonly Dictionary<PropertyType, CastingConfiguration> _availableCasts;

        public CastingService(IEnumerable<CastingConfiguration> castingConfigurations)
        {
            _availableCasts = new Dictionary<PropertyType, CastingConfiguration>();
            InitializeCasts(castingConfigurations);
        }
        
        private void InitializeCasts(IEnumerable<CastingConfiguration> castingConfigurations)
        {
            foreach (var castingConfiguration in castingConfigurations)
            {
                _availableCasts[castingConfiguration.CastTo] = castingConfiguration;
            }
        }

        public PropertyCastResult CastType(TypeInformation sourceProp, TypeInformation destinationProp)
        {
            var result = new PropertyCastResult()
            {
                SourcePropertyTypeInfo = sourceProp,
                DestinationPropertyTypeInfo = destinationProp,
            };

            if (sourceProp.HasSameCategory(destinationProp))
            {
                //collections:
                if (sourceProp is CollectionTypeInformation collectionType &&
                    !collectionType.HasSameCollectionType(destinationProp as CollectionTypeInformation))
                {
                    result.IsUnMappable = true;
                    result.Cast = null;
                    return result;
                }

                var castingFunc = GetAvailableCast(sourceProp.Type, destinationProp.Type);

                if (castingFunc != null)
                {
                    result.IsUnMappable = false;
                    result.Cast = castingFunc;
                    return result;
                }
            }

            result.IsUnMappable = true;
            result.Cast = null;
            return result;
        }

        private Func<string, string> GetAvailableCast(PropertyType sourcePropType,
            PropertyType destinationPropType)
        {
            if (_availableCasts.TryGetValue(destinationPropType, out var availableCast))
            {
                return availableCast.HasCastFrom(sourcePropType) ? availableCast.CastFunction : null;
            }

            return null;
        }
    }
}