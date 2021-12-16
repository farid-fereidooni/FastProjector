using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FastProjector.MapGenerator.SourceGeneration.Interfaces
{   
    internal interface IClassSourceText: ISourceText
    {
        IClassSourceText AddField(IFieldSourceText fieldSource);
        IClassSourceText AddProperty(IPropertySourceText propertySource);
        IClassSourceText AddMethod(IMethodSourceText methodSource);
    }
}