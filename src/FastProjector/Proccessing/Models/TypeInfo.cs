using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace FastProjector.MapGenerator.Proccessing.Models
{
    internal class TypeInformation: SubTypeInformation
    {
        public SubTypeInformation[] GenericTypes { get; }
        
        protected ITypeSymbol[] GenericSymbols { get; }

        public TypeInformation(ITypeSymbol typeSymbol)
            :base(typeSymbol)
        {
            if (!typeSymbol.IsGeneric()) return;
            
            GenericSymbols = (typeSymbol as INamedTypeSymbol)?.TypeArguments.ToArray();
            GenericTypes = GenericSymbols?.Select(sym => new SubTypeInformation(sym)).ToArray();
        }
        
        public TypeInformation(string fullName, IEnumerable<SubTypeInformation> genericTypes)
            :base(fullName)
        {
            GenericTypes = genericTypes.ToArray();
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
    }
}