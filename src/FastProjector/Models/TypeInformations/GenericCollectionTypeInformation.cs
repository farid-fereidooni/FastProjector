using System.Linq;
using FastProjector.Helpers;
using Microsoft.CodeAnalysis;

namespace FastProjector.Models.TypeInformations
{
    internal class GenericCollectionTypeInformation : CollectionTypeInformation
    {
        public GenericCollectionTypeInformation(ITypeSymbol type, PropertyType genericType) : base(type)
        {
            Type = genericType;
        }

        public GenericCollectionTypeInformation(CollectionTypeEnum collectionType,
            TypeInformation collectionGenericType)
            : base(collectionType.ConvertToPropertyTypeEnum().GetFullname(), collectionGenericType)
        {
            Type = collectionType.ConvertToPropertyTypeEnum();
        }

        public override PropertyType Type { get; }

        public override TypeInformation GetCollectionType()
        {
            return GenericTypes.FirstOrDefault();
        }
    }
}