namespace SourceCreationHelper.Interfaces
{   
    public interface INamespaceSourceText: ISourceText
    {
        INamespaceSourceText AddClass(IClassSourceText classSource);

        INamespaceSourceText AddUsing(string usingExpression);
    }
}