using System.Collections.Generic;
using System.Linq;

namespace FastProjector.MapGenerator.SourceGenerator.Interfaces
{   
    internal interface IMethodSourceText: ISourceText
    {
        IMethodSourceText AddSource(ISourceText source);
        IMethodSourceText AddSource(string source);
    }
}