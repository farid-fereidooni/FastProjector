using System;
using System.Collections;
using System.Collections.Generic;
using FastProjector.Models.TypeInformations;

namespace FastProjector.Models.Casting
{
    internal sealed class CastingConfiguration
    {
        private readonly HashSet<PropertyType> _castFromList;
        public IReadOnlyCollection<PropertyType> CastFromList => _castFromList;
        public PropertyType CastTo { get; }
        public Func<string, string> CastFunction { get; }

        public CastingConfiguration(HashSet<PropertyType> castFromList, PropertyType castTo,
            Func<string, string> castFunction)
        {
            _castFromList = castFromList;
            CastTo = castTo;
            CastFunction = castFunction;
        }

        public bool HasCastFrom(PropertyType type)
        {
            return _castFromList.Contains(type);
        }
    }
}