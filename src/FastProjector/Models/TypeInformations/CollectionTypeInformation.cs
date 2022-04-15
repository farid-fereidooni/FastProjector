using Microsoft.CodeAnalysis;

namespace FastProjector.Models.TypeInformations
{
    internal abstract class CollectionTypeInformation: TypeInformation
    {
        protected CollectionTypeInformation(ITypeSymbol type) : base(type)
        {
        }

        public abstract TypeInformation GetCollectionType();

        public bool HasSameCollectionType(CollectionTypeInformation typeInfo)
        {
            if (typeInfo == null)
                return false;
            
            return GetCollectionType().Equals(typeInfo.GetCollectionType());
        }
    }
}