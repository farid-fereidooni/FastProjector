using System.Linq;
using Microsoft.CodeAnalysis;

namespace FastProjector.Models.TypeInformations
{
    internal class GenericCollectionTypeInformation: CollectionTypeInformation
    {
        public GenericCollectionTypeInformation(ITypeSymbol type, PropertyTypeEnum genericType) : base(type)
        {
            Type = genericType;
        }

        public override PropertyTypeEnum Type { get; }
        public override TypeInformation GetCollectionType()
        {
            return GenericTypes.FirstOrDefault();
        }
    }
}