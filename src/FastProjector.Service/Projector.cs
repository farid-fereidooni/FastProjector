using System.Linq;
using FastProjector.Shared.Contracts;

namespace FastProjector
{
    public class Projector: IProjector
    {
        public IQueryable<TDestination> Project<TDestination>(IQueryable sourceQuery)
        {
            throw new System.NotImplementedException();
        }
    }
}