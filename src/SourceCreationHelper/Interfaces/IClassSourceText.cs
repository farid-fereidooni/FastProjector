namespace SourceCreationHelper.Interfaces
{   
    public interface IClassSourceText: ISourceText
    {
        IClassSourceText AddField(IFieldSourceText fieldSource);
        IClassSourceText AddProperty(IPropertySourceText propertySource);
        IClassSourceText AddMethod(IMethodSourceText methodSource);
        string Name { get; }
    }
}