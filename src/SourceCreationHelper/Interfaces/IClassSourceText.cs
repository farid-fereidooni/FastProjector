using System;
using SourceCreationHelper.Core;

namespace SourceCreationHelper.Interfaces
{   
    public interface IClassSourceText: ISourceText
    {
        IClassSourceText AddField(IFieldSourceText fieldSource);
        IClassSourceText AddProperty(IPropertySourceText propertySource);
        IClassSourceText AddMethod(IMethodSourceText methodSource);
        IClassSourceText AddConstructor(IConstructorSourceText constructorSource);
        IClassSourceText SetAsStatic(bool isStatic = true);
        IClassSourceText SetAsVirtual(bool isVirtual = true);
        string Name { get; }
    }
}