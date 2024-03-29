using SourceCreationHelper.Interfaces;

namespace SourceCreationHelper.Core
{
    internal class AssignmentSourceText: SourceTextBase, IAssignmentSourceText
    {
        private readonly ISourceText left;
        private readonly ISourceText right;

        public AssignmentSourceText(ISourceText left, ISourceText right)
        {
            this.left = left;
            this.right = right;
        }
        protected override string BuildSource()
        {
            return $"{left} = {right}";
        }
    }
}