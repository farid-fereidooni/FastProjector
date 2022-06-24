using FastProjector.Contracts;
using FastProjector.Models.TypeInformations;
using SourceCreationHelper;
using SourceCreationHelper.Interfaces;

namespace FastProjector.Models.Projections
{
    internal sealed class ClassNestedProjection : NestedProjection, IMapBasedProjection
    {
        private readonly IMapBasedProjection _innerProjection;

        public ClassNestedProjection(IMapBasedProjection innerProjection,
            CollectionTypeInformation destinationTypeInformation) : base(innerProjection, destinationTypeInformation)
        {
            _innerProjection = innerProjection;
        }

        public void AddModelMap(ModelMap modelMap)
        {
            _innerProjection.AddModelMap(modelMap);
        }

        public bool HasModelMap()
        {
            return _innerProjection.HasModelMap();
        }
    }
}