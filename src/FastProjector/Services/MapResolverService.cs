using FastProjector.Contracts;
using FastProjector.Models;
using FastProjector.Models.TypeMetaDatas;

namespace FastProjector.Services
{
    internal class MapResolverService: IMapResolverService
    {
        private readonly IMapRepository _mapCache;
        private readonly IModelMapService _mapService;

        public MapResolverService(IMapRepository mapCache, IModelMapService mapService)
        {
            _mapCache = mapCache;
            _mapService = mapService;
        }
        
        public ModelMap ResolveMap(ClassTypeMetaData sourceType, ClassTypeMetaData destinationType)
        {
            var cachedMap = _mapCache.Get(sourceType.TypeInformation, destinationType.TypeInformation);
            if (cachedMap is not null)
                return cachedMap;

            if (sourceType.TypeInformation.Equals(destinationType.TypeInformation))
            {
               return new ModelMapMetaData(sourceType.TypeSymbol, destinationType.TypeSymbol)
                    .CreateModelMap(_mapService);
            }

            return null;
        }
    }
}