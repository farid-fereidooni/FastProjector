using System.Collections.Generic;
using FastProjector.Shared.Contracts;

namespace FastProjector.Shared.Models
{   
    public class ProjectionDestinations: IProjectionDestinations
    {
        private readonly Dictionary<string, IProjectionMetadata> _destinations;

        public ProjectionDestinations()
        {
            _destinations = new Dictionary<string, IProjectionMetadata>();
        }

        public IProjectionMetadata To<TDestination>()
        {
            var fullName = typeof(TDestination).FullName;

            return fullName != null && _destinations.TryGetValue(fullName, out var metadata)
                ? metadata
                : null;
        }

        public void AddDestination(string projectionSource, IProjectionMetadata metadata)
        {
            _destinations[projectionSource] = metadata;
        }
    }
}