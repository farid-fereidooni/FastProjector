namespace SourceCreationHelper.Interfaces
{   
    public interface IBlockSourceText: ISourceText
    {
        IBlockSourceText AddSource(ISourceText source);
        IBlockSourceText AddSource(string source);
    }
}