namespace FastProjector.Models.Projections
{
    internal interface IMapBasedProjection: IProjection
    {
        public void AddModelMap(ModelMap modelMap);

    }
}