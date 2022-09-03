using System.Collections.Generic;
using FastProjector.Shared.Contracts;

namespace FastProjector.Shared.Models
{
    public class ProjectionConfiguration : IProjectionConfiguration
    {
        private readonly Dictionary<string, IProjectionDestinations> _configurations;

        public ProjectionConfiguration()
        {
            _configurations = new Dictionary<string, IProjectionDestinations>();
        }

        public IProjectionDestinations From<TSource>()
        {
            var fullName = typeof(TSource).FullName;

            return fullName != null && _configurations.TryGetValue(fullName, out var destinations)
                ? destinations
                : null;
        }

        public void AddConfig(string projectionSource, IProjectionDestinations destinations)
        {
            _configurations[projectionSource] = destinations;
        }
    }
}