using System;
using Microsoft.CodeAnalysis;

namespace FastProjector.Models.TypeInformations
{
    internal sealed class ArrayTypeInformation : CollectionTypeInformation
    {
        private readonly TypeInformation _arrayType;

        public ArrayTypeInformation(ITypeSymbol type) : base(type)
        {
            if (type is not IArrayTypeSymbol arraySymbol)
                throw new Exception("Type should be array type");
            
            Type = PropertyTypeEnum.System_Array;
            _arrayType = Create(arraySymbol.ElementType);
        }

        public override PropertyTypeEnum Type { get; }
        public override TypeInformation GetCollectionType()
        {
            return _arrayType;
        }
    }
}