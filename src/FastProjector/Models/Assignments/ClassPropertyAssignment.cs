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

            var castResult = mapService.CastType(_sourceType.TypeMetaData.TypeInformation,
                _destinationType.TypeMetaData.TypeInformation);

            if (castResult.IsUnMapable) return null;
            
            var castedSourceText = SourceCreator.CreateSource(castResult.Cast(fullParamName.Text));
            return CreateAssignment(castedSourceText);

        }

        private IAssignmentSourceText CreateAssignment(ISourceText mapSourceText)
        {
            return SourceCreator.CreateAssignment(
                SourceCreator.CreateSource(_destinationType.PropertyName),
                mapSourceText
            );
        }

        public override (ClassTypeMetaData sourceType, ClassTypeMetaData destinationType) GetRequiredMapTypes()
        {
            return (_sourceType.TypeMetaData, _destinationType.TypeMetaData);
        }
    }
}