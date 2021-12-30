using System.Collections.Generic;
using System.Linq;

namespace FastProjector.MapGenerator.SourceGeneration.Interfaces
{   
    internal interface ICallSourceText: ISourceText
    {
        ICallSourceText AddArgument(ISourceText argument);
    }
}