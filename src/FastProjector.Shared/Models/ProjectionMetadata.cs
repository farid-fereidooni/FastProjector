using FastProjector.Shared.Contracts;

namespace FastProjector.Shared.Models
{
    public class ProjectionMetadata: IProjectionMetadata
    {
        public ProjectionMetadata(IQueryableProjectionMetadata queryableProjectionMetadata)
        {
            QueryableProjectionMetadata = queryableProjectionMetadata;
        }
        public IQueryableProjectionMetadata QueryableProjectionMetadata { get; }
    }
}