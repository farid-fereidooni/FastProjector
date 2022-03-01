namespace SourceCreationHelper.Interfaces
{
    public interface IMemberInitSourceText: ISourceText
    {
        IMemberInitSourceText AddAssignment(IAssignmentSourceText assignment);
    }
}