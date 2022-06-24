using FastProjector.Contracts;
using FastProjector.Models.Projections;
using SourceCreationHelper.Interfaces;

namespace FastProjector.Models.Assignments
{
    internal class PrimitiveCollectionPropertyAssignment: PropertyAssignment
    {
        public PrimitiveCollectionPropertyAssignment(IProjection projection)
        { }
        
        public override IAssignmentSourceText CreateAssignmentSource(IModelMapService mapService, ISourceText parameterName)
        {
            throw new System.NotImplementedException();
        }
    }
}