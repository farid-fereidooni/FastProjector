using System;
using FastProjector.Contracts;
using FastProjector.Models.TypeMetaDatas;
using SourceCreationHelper;
using SourceCreationHelper.Interfaces;

namespace FastProjector.Models.Projections
{
    internal sealed class ClassProjection : Projection, IMapBasedProjection
    {
        private readonly CollectionTypeMetaData _sourceTypeMetaData;
        private readonly CollectionTypeMetaData _destinationTypeMetadata;

        public ClassProjection(CollectionTypeMetaData sourceTypeMetaData,
            CollectionTypeMetaData destinationTypeMetadata)
            : base(destinationTypeMetadata.TypeInformation)
        {
            _sourceTypeMetaData = sourceTypeMetaData;
            _destinationTypeMetadata = destinationTypeMetadata;
        }

        public override ISourceText CreateProjection(IModelMapService mapService, ISourceText parameterName)
        {
            if (ModelMap is null)
                throw new InvalidOperationException("No model map has been passed");
            var projectionParam = SourceCreator.CreateSource(mapService.GetNewProjectionVariableName());

            var modelMapSource = ModelMap.CreateMappingSource(mapService, projectionParam);
            var projectionSource = CreateSelectExpression(projectionParam.Text, modelMapSource);

            var enumerableCastInfo = CreateIEnumerableCasting(mapService);
            
            if (enumerableCastInfo.IsUnMapable)
                return null;

            return SourceCreator.CreateSource(
                enumerableCastInfo.Cast($"{parameterName}.{projectionSource}"));
        }

        public ModelMap ModelMap { get; private set; }

        public void AddModelMap(ModelMap modelMap)
        {
            ValidateMap();
            ModelMap = modelMap;
        }

        public (TypeMetaData sourceType, TypeMetaData destinationType) GetRequiredMapTypes()
        {
            return (_sourceTypeMetaData, _destinationTypeMetadata);
        }

        public bool HasModelMap()
        {
            return ModelMap is not null;
        }

        private void ValidateMap()
        {
            if(ModelMap is null)
                return;
            if (!ModelMap.SourceType.Equals(_sourceTypeMetaData.GetCollectionType().TypeInformation))
                throw new ArgumentException("Invalid Metadata passed");

            if (!ModelMap.DestinationType.Equals(_destinationTypeMetadata.GetCollectionType().TypeInformation))
                throw new ArgumentException("Invalid Metadata passed");
        }
    }
}