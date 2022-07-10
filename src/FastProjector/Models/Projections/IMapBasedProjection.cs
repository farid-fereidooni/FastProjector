using FastProjector.Models.TypeMetaDatas;

namespace FastProjector.Models.Projections
{
    internal interface IMapBasedProjection: IProjection
    {
        public void AddModelMap(ModelMap modelMap);
        public (TypeMetaData sourceType, TypeMetaData destinationType) GetRequiredMapTypes();
        public bool HasModelMap();

    }
}