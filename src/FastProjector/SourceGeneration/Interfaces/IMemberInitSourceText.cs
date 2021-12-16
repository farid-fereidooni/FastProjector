namespace FastProjector.MapGenerator.SourceGeneration.Interfaces
{
    internal interface IMemberInitSourceText: ISourceText
    {
        IMemberInitSourceText AddAssignment(IAssignmentSourceText assignment);
    }
}