using FastProjector.Contracts;
using SourceCreationHelper.Interfaces;

namespace FastProjector.Models.Projections
{
    internal interface IProjection
    {
        ISourceText CreateProjection(IModelMapService mapService, ISourceText parameterName);
    }
}