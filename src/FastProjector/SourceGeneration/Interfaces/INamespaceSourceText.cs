using FastProjector.MapGenerator.SourceGeneration.Interfaces;

namespace FastProjector.MapGenerator.SourceGeneration.Interfaces
{   
    internal interface INamespaceSourceText: ISourceText
    {
        INamespaceSourceText AddClass(IClassSourceText classSource);

        INamespaceSourceText AddUsing(string usingExpression);
    }
}