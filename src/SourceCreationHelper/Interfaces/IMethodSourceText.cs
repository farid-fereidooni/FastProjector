namespace SourceCreationHelper.Interfaces
{   
    public interface IMethodSourceText: ISourceText
    {
        string Name { get; }
        IMethodSourceText AddSource(ISourceText source);
        IMethodSourceText AddSource(string source);
        IMethodSourceText AddParameter(string type, string name);
        IMethodSourceText SetAsStatic(bool isStatic = true);
        IMethodSourceText SetAsVirtual(bool isVirtual = true);
        IMethodSourceText SetAsAsync(bool isAsync = true);
    }
}