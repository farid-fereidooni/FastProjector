using System.Collections.Generic;

namespace SourceCreationHelper.Interfaces
{   
    public interface IConstructorSourceText: ISourceText
    {
        IConstructorSourceText AddSource(ISourceText source);
        IConstructorSourceText AddSource(string source);
        IConstructorSourceText AddParameter(string type, string name);
        IConstructorSourceText SetAsStatic(bool isStatic = true);
        bool IsStatic { get; }
        string Name { get;}
        
        IReadOnlyCollection<string> Parameters { get; }
    }
}