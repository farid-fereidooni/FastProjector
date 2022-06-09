using System;
using FastProjector.Helpers;
using Microsoft.CodeAnalysis;

namespace FastProjector.Models.TypeInformations
{
    internal sealed class ArrayTypeInformation : CollectionTypeInformation
    {
        private readonly TypeInformation _arrayType;

        public ArrayTypeInformation(ITypeSymbol type) : base(type)
        {
            if (type is not IArrayTypeSymbol arraySymbol)
                throw new ArgumentException("Type should be array type");
            
            Type = PropertyType.System_Array;
            _arrayType = Create(arraySymbol.ElementType);
        }
        
        public ArrayTypeInformation(TypeInformation arrayType) : base(PropertyType.System_Array.GetFullname())
        {
            Type = PropertyType.System_Array;
            _arrayType = arrayType;
        }
        
        public override PropertyType Type { get; }
        public override TypeInformation GetCollectionType()
        {
            return _arrayType;
        }
    }
}