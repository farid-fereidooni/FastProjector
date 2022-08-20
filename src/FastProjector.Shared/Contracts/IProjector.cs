using System.Linq;

namespace FastProjector.Shared.Contracts
{
    public interface IProjector
    {
        IQueryable<TDestination> Project<TDestination>(IQueryable sourceQuery);
    }
}