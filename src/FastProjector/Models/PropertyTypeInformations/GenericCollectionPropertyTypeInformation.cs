using System.Linq;
using Microsoft.CodeAnalysis;

namespace FastProjector.Models.PropertyTypeInformations
{
    internal class GenericCollectionPropertyTypeInformation: CollectionPropertyTypeInformation
    {
        public GenericCollectionPropertyTypeInformation(ITypeSymbol type, PropertyTypeEnum genericType) : base(type)
        {
            Type = genericType;
        }

        public override PropertyTypeEnum Type { get; }
        public override SubTypeInformation GetCollectionType()
        {
            return GenericTypes.FirstOrDefault();
        }
    }
}