using System.Collections.Generic;
using System.Text;
using FastProjector.MapGenerator.SourceGeneration.Interfaces;

namespace FastProjector.SourceGeneration.Core
{
    internal class MemberInitSourceText: SourceTextBase, IMemberInitSourceText
    {

        private readonly List<IAssignmentSourceText> assignments;

        public MemberInitSourceText()
        {
            assignments = new List<IAssignmentSourceText>();
        }

        public MemberInitSourceText(params IAssignmentSourceText[] assignments)
        {
            this.assignments = new List<IAssignmentSourceText>(assignments);
        }
        
        public MemberInitSourceText(IEnumerable<IAssignmentSourceText> assignments)
        {
            this.assignments = new List<IAssignmentSourceText>(assignments);
        }
        
        
        public IMemberInitSourceText AddAssignment(IAssignmentSourceText assignment)
        {
            assignments.Add(assignment);
            return this;
        }
        
        protected override string BuildSource()
        {
            var sourceStringBuilder = new StringBuilder();
            sourceStringBuilder.AppendLine("{");
            sourceStringBuilder.AppendLine(string.Join(",\n", assignments));
            sourceStringBuilder.AppendLine("}");
            return sourceStringBuilder.ToString();
        }
    }
}