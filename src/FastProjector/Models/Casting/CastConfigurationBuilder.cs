using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace FastProjector.Models.Casting
{
    internal sealed class CastConfigurationBuilder : ICanSetCastTo, ICanSetCastFunction,
        ICanSetCastFunctionWithMultipleTo
    {
        private readonly HashSet<PropertyType> _castFromList;
        private readonly HashSet<PropertyType> _castToList;
        private PropertyType _castTo;

        private CastConfigurationBuilder()
        {
            _castFromList = new HashSet<PropertyType>();
            _castToList = new HashSet<PropertyType>();
        }

        public static ICanSetCastFrom CreateCast()
        {
            return new CastConfigurationBuilder();
        }

        public ICanSetCastTo From(PropertyType type)
        {
            if (!Enum.IsDefined(typeof(PropertyType), type))
                throw new InvalidEnumArgumentException(nameof(type), (int) type, typeof(PropertyType));

            _castFromList.Add(type);
            return this;
        }

        public ICanSetCastFunction To(PropertyType type)
        {
            if (!Enum.IsDefined(typeof(PropertyType), type))
                throw new InvalidEnumArgumentException(nameof(type), (int) type, typeof(PropertyType));

            _castTo = type;
            return this;
        }

        ICanSetCastFunctionWithMultipleTo ICanSetMultipleTo.To(PropertyType type)
        {
            if (!Enum.IsDefined(typeof(PropertyType), type))
                throw new InvalidEnumArgumentException(nameof(type), (int) type, typeof(PropertyType));

            _castToList.Add(type);
            return this;
        }

        public CastingConfiguration With(Func<string, string> castFunction)
        {
            if (castFunction == null) throw new ArgumentNullException(nameof(castFunction));

            return new CastingConfiguration(_castFromList, _castTo, castFunction);
        }

        IEnumerable<CastingConfiguration> ICanSetCastFunctionWithMultipleTo.With(Func<string, string> castFunction)
        {
            if (castFunction == null) throw new ArgumentNullException(nameof(castFunction));
            _castToList.Add(_castTo);
            return _castToList.Select(castToItem => new CastingConfiguration(_castFromList, castToItem, castFunction));
        }
    }
}