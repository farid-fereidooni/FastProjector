using FastProjector.Contracts;
using FastProjector.Models.TypeInformations;
using SourceCreationHelper;
using SourceCreationHelper.Interfaces;

namespace FastProjector.Models.Projections
{
    internal sealed class ClassNestedProjection : NestedProjection, IMapBasedProjection
    {
        private readonly MapBasedProjection _innerProjection;

        public ClassNestedProjection(MapBasedProjection innerProjection,
            CollectionTypeInformation destinationTypeInformation) : base(innerProjection, destinationTypeInformation)
        {
            _innerProjection = innerProjection;
        }

        public void AddModelMap(ModelMap modelMap)
        {
            _innerProjection.AddModelMap(modelMap);
        }
    }
}