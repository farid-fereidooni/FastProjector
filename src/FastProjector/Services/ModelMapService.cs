using FastProjector.Contracts;
using FastProjector.Helpers;
using FastProjector.Models;
using FastProjector.Models.TypeInformations;
using Microsoft.CodeAnalysis;
using SourceCreationHelper.Interfaces;

namespace FastProjector.Services
{
    internal class ModelMapService : IModelMapService
    {
        private readonly IMapCache _mapCache;
        private readonly ICastingService _castingService;
        private readonly IVariableNameGenerator _nameGenerator;

        public ModelMapService(IMapCache mapCache, ICastingService castingService, IVariableNameGenerator nameGenerator)
        {
            _mapCache = mapCache;
            _castingService = castingService;
            _nameGenerator = nameGenerator;
        }

        
        public ModelMap FetchFromCache(TypeInformation sourceType, TypeInformation destinationType)
        {
            return _mapCache.Get(sourceType, destinationType);
        }

        public void AddToCache(ModelMap modelMap)
        {
            _mapCache.Add(modelMap);
        }


        public PropertyCastResult CastType(TypeInformation sourceType,
            TypeInformation destinationType)
        {
            return _castingService.CastType(sourceType, destinationType);
        }

        public string GetNewProjectionVariableName()
        {
            return _nameGenerator.GetNew();
        }
    }
}