using FastProjector.MapGenerator.SourceGenerator.Interfaces;

namespace FastProjector.MapGenerator.SourceGenerator.Interfaces
{   
    internal interface INamespaceSourceText: ISourceText
    {
        INamespaceSourceText AddClass(IClassSourceText classSource);

        INamespaceSourceText AddUsing(string usingExpression);
    }
}