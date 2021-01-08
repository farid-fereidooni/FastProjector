using FastProjector.MapGenerator.SourceGenerator.Interfaces;

namespace FastProjector.MapGenerator.SourceGenerator.Interfaces
{   
    internal interface INamespaceSourceText: ISourceText
    {
        void AddClass(string name, AccessModifier accessModifier, bool isStatic = false, bool isVirtual = false);
    }
}