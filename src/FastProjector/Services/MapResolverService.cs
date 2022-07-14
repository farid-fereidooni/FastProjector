using FastProjector.Contracts;
using FastProjector.Models;
using FastProjector.Models.TypeMetaDatas;

namespace FastProjector.Services
{
    internal class MapResolverService: IMapResolverService
    {
        private readonly IMapRepository _mapRepository;

        public MapResolverService(IMapRepository mapRepository)
        {
            _mapRepository = mapRepository;
        }
        
        public ModelMap ResolveMap(ClassTypeMetaData sourceType, ClassTypeMetaData destinationType)
        {
            var cachedMap = _mapRepository.Get(sourceType.TypeInformation, destinationType.TypeInformation);
            if (cachedMap is not null)
                return cachedMap;

            if (sourceType.TypeInformation.Equals(destinationType.TypeInformation))
            {
                var modelMapMetadata = new ModelMapMetaData(sourceType.TypeSymbol, destinationType.TypeSymbol);
               return new ModelMap(modelMapMetadata);
            }

            return null;
        }
    }
}