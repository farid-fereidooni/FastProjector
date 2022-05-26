using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using FastProjector.Models.TypeInformations;

namespace FastProjector.Models.Casting
{
    internal sealed class CastingConfiguration
    {
        private readonly HashSet<PropertyTypeEnum> _castFromList;
        public IReadOnlyCollection<PropertyTypeEnum> CastFromList => _castFromList;
        public PropertyTypeEnum CastTo { get; }
        public Func<string, string> CastFunction { get; }

        public CastingConfiguration(HashSet<PropertyTypeEnum> castFromList, PropertyTypeEnum castTo, Func<string, string> castFunction)
        {
            _castFromList = castFromList;
            CastTo = castTo;
            CastFunction = castFunction;
        }

        public bool HasCastFrom(PropertyTypeEnum type)
        {
            return _castFromList.Contains(type);
        }
    }

    internal sealed class CastConfigurationBuilder : ICanSetCastTo, ICanSetCastFunction
    {
        private readonly HashSet<PropertyTypeEnum> _castFromList;
        private PropertyTypeEnum _castTo;

        private CastConfigurationBuilder()
        {
            _castFromList = new HashSet<PropertyTypeEnum>();
        }

        public static ICanSetCastFrom CreateCast()
        {
            return new CastConfigurationBuilder();
        }

        public ICanSetCastTo From(PropertyTypeEnum type)
        {
            if (!Enum.IsDefined(typeof(PropertyTypeEnum), type))
                throw new InvalidEnumArgumentException(nameof(type), (int) type, typeof(PropertyTypeEnum));

            _castFromList.Add(type);
            return this;
        }

        public ICanSetCastFunction To(PropertyTypeEnum type)
        {
            if (!Enum.IsDefined(typeof(PropertyTypeEnum), type))
                throw new InvalidEnumArgumentException(nameof(type), (int) type, typeof(PropertyTypeEnum));
            
            _castTo = type;
            return this;
        }

        public CastingConfiguration With(Func<string, string> castFunction)
        {
            if (castFunction == null) throw new ArgumentNullException(nameof(castFunction));

            return new CastingConfiguration(_castFromList, _castTo, castFunction);
        }
    }

    internal interface ICanSetCastFrom
    {
        ICanSetCastTo From(PropertyTypeEnum type);
    }

    internal interface ICanSetCastTo : ICanSetCastFrom
    {
        ICanSetCastFunction To(PropertyTypeEnum type);
    }

    internal interface ICanSetCastFunction
    {
        CastingConfiguration With(Func<string, string> castFunction);
    }
}