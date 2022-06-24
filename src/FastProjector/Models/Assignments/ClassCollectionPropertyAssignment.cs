using FastProjector.Contracts;
using SourceCreationHelper.Interfaces;

namespace FastProjector.Models.Assignments
{
    internal class ClassCollectionPropertyAssignment: MapBasedPropertyAssignments
    {
        public override IAssignmentSourceText CreateAssignmentSource(IModelMapService mapService, ISourceText parameterName)
        {
            throw new System.NotImplementedException();
        }

        protected override void ValidateMap()
        {
            throw new System.NotImplementedException();
        }
    }
}