using Microsoft.CodeAnalysis;

namespace FastProjector.Models.PropertyTypeInformations
{
    internal abstract class CollectionPropertyTypeInformation: PropertyTypeInformation
    {
        protected CollectionPropertyTypeInformation(ITypeSymbol type) : base(type)
        {
        }

        public abstract SubTypeInformation GetCollectionType();

        public bool HasSameCollectionType(CollectionPropertyTypeInformation typeInfo)
        {
            if (typeInfo == null)
                return false;
            
            return GetCollectionType().Equals(typeInfo.GetCollectionType());
        }
    }
}