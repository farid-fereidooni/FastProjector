using FastProjector.Contracts;
using FastProjector.Models;
using FastProjector.Models.Casting;
using FastProjector.Models.TypeInformations;

namespace FastProjector.Services
{
    internal class ModelMapService : IModelMapService
    {
        private readonly ICastingService _castingService;
        private readonly IVariableNameGenerator _nameGenerator;

        public ModelMapService(ICastingService castingService, IVariableNameGenerator nameGenerator)
        {
            _castingService = castingService;
            _nameGenerator = nameGenerator;
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