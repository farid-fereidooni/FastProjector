namespace SourceCreationHelper.Interfaces
{   
    public interface IMethodSourceText: ISourceText
    {
        IMethodSourceText AddSource(ISourceText source);
        IMethodSourceText AddSource(string source);
    }
}