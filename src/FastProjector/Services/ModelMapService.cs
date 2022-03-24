using FastProjector.Contracts;
using FastProjector.Helpers;
using FastProjector.Models;
using FastProjector.Models.PropertyTypeInformations;
using Microsoft.CodeAnalysis;
using SourceCreationHelper.Interfaces;

namespace FastProjector.Services
{
    internal class ModelMapService : IModelMapService
    {
        private readonly IMapCache _mapCache;
        private readonly ICastingService _castingService;

        public ModelMapService(IMapCache mapCache, ICastingService castingService)
        {
            _mapCache = mapCache;
            _castingService = castingService;
        }

        
        public ISourceText FetchFromCache(PropertyTypeInformation sourceType, PropertyTypeInformation destinationType)
        {
            return _mapCache.Get(sourceType, destinationType);
        }
        
        public ModelMapMetaData CreateSameTypeMap(ITypeSymbol type, int level)
        {
            return new ModelMapMetaData(type, type);
        }

        public PropertyCastResult CastType(PropertyTypeInformation sourceType,
            PropertyTypeInformation destinationType)
        {
            return _castingService.CastType(sourceType, destinationType);
        }
    }
}