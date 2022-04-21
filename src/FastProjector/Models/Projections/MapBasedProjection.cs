using SourceCreationHelper;
using SourceCreationHelper.Interfaces;

namespace FastProjector.Models.Projections
{
    internal abstract class MapBasedProjection: Projection
    {
        protected MapBasedProjection(ModelMap modelMap)
        {
            ModelMap = modelMap;
        }
        protected ModelMap ModelMap { get; }

        protected abstract void ValidateMap();
        
      
    }
}