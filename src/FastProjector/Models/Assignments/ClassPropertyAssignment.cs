using System;
using FastProjector.Contracts;
using FastProjector.Models.PropertyMetadatas;
using FastProjector.Models.TypeMetaDatas;
using Microsoft.CodeAnalysis;
using SourceCreationHelper;
using SourceCreationHelper.Interfaces;

namespace FastProjector.Models.Assignments
{
    internal class ClassPropertyAssignment: MapBasedPropertyAssignments
    {
        private readonly ClassPropertyMetadata _sourceType;
        private readonly ClassPropertyMetadata _destinationType;

        public ClassPropertyAssignment(ClassPropertyMetadata sourceType, ClassPropertyMetadata destinationType)
        {
            _sourceType = sourceType;
            _destinationType = destinationType;
        }
        
        public override IAssignmentSourceText CreateAssignmentSource(IModelMapService mapService, ISourceText parameterName)
        {
            var fullParamName = SourceCreator.CreateSource($"{parameterName}.{_sourceType.PropertyName}");

            if (ModelMap is not null)
                return CreateAssignment(ModelMap.CreateMappingSource(mapService, fullParamName));
            
            var cachedMapping = mapService.FetchFromCache(_sourceType.TypeMetaData.TypeInformation,
                _destinationType.TypeMetaData.TypeInformation);

            if (cachedMapping is not null)
                return CreateAssignment(cachedMapping.CreateMappingSource(mapService, fullParamName));

            var sameTypeMap = TryCreateSameTypeMap(mapService);
            
            if (sameTypeMap is not null)
                return CreateAssignment(sameTypeMap.CreateMappingSource(mapService, fullParamName));

            var castResult = mapService.CastType(_sourceType.TypeMetaData.TypeInformation,
                _destinationType.TypeMetaData.TypeInformation);

            if (!castResult.IsUnMapable)
            {
                var castedSourceText = SourceCreator.CreateSource(castResult.Cast(fullParamName.Text));
                return CreateAssignment(castedSourceText);
            }

            return null;
        }
        public override void AddModelMap(ModelMap modelMap)
        {
            ValidateMap();
            base.AddModelMap(modelMap);
        }

        private ModelMap TryCreateSameTypeMap(IModelMapService mapService)
        {
            if (!_sourceType.TypeMetaData.TypeInformation.Equals(_destinationType.TypeMetaData.TypeInformation))
                return null;
            
            var modelMap = new ModelMapMetaData(_sourceType.TypeMetaData.TypeSymbol,
                _destinationType.TypeMetaData.TypeSymbol).CreateModelMap(mapService);
            
            return modelMap.CheckIfMappingPossible() ? modelMap : null;
        }

        private IAssignmentSourceText CreateAssignment(ISourceText mapSourceText)
        {
            return SourceCreator.CreateAssignment(
                SourceCreator.CreateSource(_destinationType.PropertyName),
                mapSourceText
            );
        }

        private void ValidateMap()
        {
            if(ModelMap is null)
                return;
            
            if (!ModelMap.SourceType.Equals(_sourceType.TypeMetaData.TypeInformation))
                throw new ArgumentException("Invalid Metadata passed");
            
            if (!ModelMap.DestinationType.Equals(_destinationType.TypeMetaData.TypeInformation))
                throw new ArgumentException("Invalid Metadata passed");
        }


    }
}