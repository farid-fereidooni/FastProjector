using Microsoft.CodeAnalysis;

namespace FastProjector.Models.TypeInformations
{
    internal abstract class CollectionTypeInformation: TypeInformation
    {
        protected CollectionTypeInformation(ITypeSymbol type) : base(type)
        {
        }
        
        protected CollectionTypeInformation(string fullname)
            : base(fullname, null)
        { }

        protected CollectionTypeInformation(string fullname, TypeInformation genericType)
            : base(fullname, new[] {genericType})
        { }

        public abstract TypeInformation GetCollectionType();

        public bool HasSameCollectionType(CollectionTypeInformation typeInfo)
        {
            if (typeInfo == null)
                return false;
            
            return GetCollectionType().Equals(typeInfo.GetCollectionType());
        }
    }
}