namespace SourceCreationHelper.Interfaces
{   
    public interface ICallSourceText: ISourceText
    {
        ICallSourceText AddArgument(ISourceText argument);
    }
}