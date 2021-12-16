using System;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace FastProjector.MapGenerator.Proccessing.Models
{
    internal class TypeInformation :SubTypeInformation
    {
        public TypeInformation(ITypeSymbol typeSymbol)
        :base(typeSymbol)
        {
            if (typeSymbol.IsGeneric())
            {
                GenericTypes = (typeSymbol as INamedTypeSymbol)?.TypeArguments.Select(sym => new SubTypeInformation(sym)).ToArray();
            }
        }

        public bool IsSameAs(TypeInformation other)
        {
            return Equals(other);
        }

        public override bool Equals(object obj)
        {
            if (obj is TypeInformation other)
            {
                return base.Equals(other) && HasSameGenerics(other);
            }
            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ (GenericTypes != null ? GenericTypes.GetHashCode() : 0);
            }
        }

        public bool HasSameGenerics(TypeInformation typeInfo)
        {
            if (typeInfo.GenericTypes == null && GenericTypes == null)
                return true;

            if (typeInfo.GenericTypes != null && GenericTypes != null)
            {
                return GenericTypes.SequenceEqual(typeInfo.GenericTypes);
            }

            return false;

        }

        public SubTypeInformation[] GenericTypes { get; }
    }
}