using FastProjector.Models.TypeMetaDatas;

namespace FastProjector.Models.Projections
{
    internal interface IMapBasedProjection: IProjection, IRequireMapData
    {
        public void AddModelMap(ModelMap modelMap);
        public bool HasModelMap();

    }
}