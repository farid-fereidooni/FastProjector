namespace SourceCreationHelper.Interfaces
{   
    public interface IFieldSourceText: ISourceText
    {
        IFieldSourceText AddInitializer(ISourceText source);
    }
}