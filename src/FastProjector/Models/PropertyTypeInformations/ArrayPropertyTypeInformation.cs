using System;
using Microsoft.CodeAnalysis;

namespace FastProjector.Models.PropertyTypeInformations
{
    internal sealed class ArrayPropertyTypeInformation : CollectionPropertyTypeInformation
    {
        private readonly SubTypeInformation _arrayType;

        public ArrayPropertyTypeInformation(ITypeSymbol type) : base(type)
        {
            if (type is not IArrayTypeSymbol arraySymbol)
                throw new Exception("Type should be array type");
            
            Type = PropertyTypeEnum.System_Array;
            _arrayType = new SubTypeInformation(arraySymbol.ElementType);
        }

        public override PropertyTypeEnum Type { get; }
        public override SubTypeInformation GetCollectionType()
        {
            return _arrayType;
        }
    }
}