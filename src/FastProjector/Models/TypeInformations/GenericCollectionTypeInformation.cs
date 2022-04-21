using System.Linq;
using FastProjector.Helpers;
using Microsoft.CodeAnalysis;

namespace FastProjector.Models.TypeInformations
{
    internal class GenericCollectionTypeInformation : CollectionTypeInformation
    {
        public GenericCollectionTypeInformation(ITypeSymbol type, PropertyTypeEnum genericType) : base(type)
        {
            Type = genericType;
        }

        public GenericCollectionTypeInformation(CollectionTypeEnum collectionType,
            TypeInformation collectionGenericType)
            : base(collectionType.ConvertToPropertyTypeEnum().GetFullname(), collectionGenericType)
        {
            Type = collectionType.ConvertToPropertyTypeEnum();
        }

        public override PropertyTypeEnum Type { get; }

        public override TypeInformation GetCollectionType()
        {
            return GenericTypes.FirstOrDefault();
        }
    }
}