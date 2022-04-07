using System;
using FastProjector.Contracts;
using FastProjector.Models.PropertyMetaDatas;
using Microsoft.CodeAnalysis;
using SourceCreationHelper;
using SourceCreationHelper.Interfaces;

namespace FastProjector.Models.Assignments
{
    internal class ClassPropertyAssignment: MapBasedPropertyAssignments
    {
        private readonly ClassPropertyMetaData _sourceProperty;
        private readonly ClassPropertyMetaData _destinationProperty;

        public ClassPropertyAssignment(ClassPropertyMetaData sourceProperty, ClassPropertyMetaData destinationProperty)
        {
            _sourceProperty = sourceProperty;
            _destinationProperty = destinationProperty;
        }
        
        public override IAssignmentSourceText CreateAssignmentSource(IModelMapService mapService, ISourceText parameterName)
        {
            var fullParamName = SourceCreator.CreateSource($"{parameterName}.{_sourceProperty.GetPropertyName()}");

            if (ModelMap is not null)
                return CreateAssignment(ModelMap.CreateMappingSource(mapService, fullParamName));
            
            var cachedMapping = mapService.FetchFromCache(_sourceProperty.PropertyTypeInformation,
                _destinationProperty.PropertyTypeInformation);

            if (cachedMapping is not null)
                return CreateAssignment(cachedMapping.CreateMappingSource(mapService, fullParamName));

            var sameTypeMap = TryCreateSameTypeMap(mapService);
            
            if (sameTypeMap is not null)
                return CreateAssignment(sameTypeMap.CreateMappingSource(mapService, fullParamName));

            var castResult = mapService.CastType(_sourceProperty.PropertyTypeInformation,
                _destinationProperty.PropertyTypeInformation);

            if (!castResult.IsUnMapable)
            {
                var castedSourceText = SourceCreator.CreateSource(castResult.Cast(fullParamName.Text));
                return CreateAssignment(castedSourceText);
            }

            return null;
        }

        private ModelMap TryCreateSameTypeMap(IModelMapService mapService)
        {
            if (!_sourceProperty.PropertyTypeInformation.Equals(_destinationProperty.PropertyTypeInformation))
                return null;
            
            var modelMap = new ModelMapMetaData(_sourceProperty.PropertySymbol.Type,
                _destinationProperty.PropertySymbol.Type).CreateModelMap(mapService);
            
            return modelMap.CheckIfMappingPossible() ? modelMap : null;
        }

        private IAssignmentSourceText CreateAssignment(ISourceText mapSourceText)
        {
            return SourceCreator.CreateAssignment(
                SourceCreator.CreateSource(_destinationProperty.GetPropertyName()),
                mapSourceText
            );
        }

        public override bool CanMapLater()
        {
            return !_sourceProperty.Equals(_destinationProperty);
        }

        protected override void ValidateMap()
        {
            if(ModelMap is null)
                return;
            
            if (!ModelMap.SourceType.Equals(_sourceProperty.PropertyTypeInformation))
                throw new ArgumentException("Invalid Metadata passed");
            
            if (!ModelMap.DestinationType.Equals(_destinationProperty.PropertyTypeInformation))
                throw new ArgumentException("Invalid Metadata passed");
        }
    }
}