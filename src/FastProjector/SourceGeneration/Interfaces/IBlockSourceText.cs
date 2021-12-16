namespace FastProjector.MapGenerator.SourceGeneration.Interfaces
{   
    internal interface IBlockSourceText: ISourceText
    {
        IBlockSourceText AddSource(ISourceText source);
        IBlockSourceText AddSource(string source);
    }
}