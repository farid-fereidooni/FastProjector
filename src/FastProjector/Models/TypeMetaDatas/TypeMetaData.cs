using FastProjector.Models.TypeInformations;
using Microsoft.CodeAnalysis;

namespace FastProjector.Models.TypeMetaDatas
{
    internal abstract class TypeMetaData
    {
        protected TypeMetaData(ITypeSymbol typeSymbol, TypeInformation typeInformation)
        {
            TypeSymbol = typeSymbol;
            TypeInformation = typeInformation;
        }
        public TypeInformation TypeInformation { get; }
        public ITypeSymbol TypeSymbol { get; }

        public virtual bool HasSameTypeCategory(TypeMetaData typeMetaData)
        {
            return TypeInformation.HasSameCategory(typeMetaData.TypeInformation);
        }

        public override bool Equals(object obj)
        {
            if (obj is TypeMetaData other)
            {
                return TypeInformation.Equals(other.TypeInformation);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return TypeInformation.GetHashCode();
        }
        
        public static TypeMetaData Create(ITypeSymbol typeSymbol)
        {
            var propertyTypeInformation = TypeInformation.Create(typeSymbol);
            if (propertyTypeInformation is null)
                return null;

            return propertyTypeInformation switch
            {
                GenericClassTypeInformation genericClassType => null,
                ClassTypeInformation classType => new ClassTypeMetaData(typeSymbol, classType),
                ArrayTypeInformation arrayType => new ArrayTypeMetaData(typeSymbol, arrayType),
                GenericCollectionTypeInformation genericCollectionType => new GenericCollectionTypeMetaData(typeSymbol, genericCollectionType),
                PrimitiveTypeInformation primitiveType => new PrimitiveTypeMetaData(typeSymbol, primitiveType),
                _ => null
            };
        }
    }
}