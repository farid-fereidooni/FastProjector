using System;
using System.Collections.Generic;
using System.Linq;
using FastProjector.Helpers;
using Microsoft.CodeAnalysis;

namespace FastProjector.Models.TypeInformations
{
    internal abstract class BaseTypeInformation
    {
        public string FullName { get; }
        public TypeInformation[] GenericTypes { get; }

        protected BaseTypeInformation(ITypeSymbol typeSymbol)
        {
            FullName = typeSymbol.GetFullName();

            if (!typeSymbol.IsGeneric()) return;
            var genericSymbols = (typeSymbol as INamedTypeSymbol)?.TypeArguments.ToArray();
            GenericTypes = genericSymbols?.Select(TypeInformation.Create).ToArray();
        }

        protected BaseTypeInformation(string fullName, IEnumerable<TypeInformation> genericTypes)
        {
            FullName = fullName;
            GenericTypes = genericTypes?.ToArray();
        }

        public override bool Equals(object obj)
        {
            if (obj is BaseTypeInformation res)
            {
                return string.Equals(FullName, res.FullName, StringComparison.CurrentCultureIgnoreCase) && HasSameGenerics(res);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return FullName.GetHashCode() ^ (GenericTypes != null ? GenericTypes.GetHashCode() : 0);
        }

        public bool HasSameGenerics(BaseTypeInformation typeInfo)
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