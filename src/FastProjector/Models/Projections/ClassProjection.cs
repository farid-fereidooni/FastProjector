using System;
using FastProjector.Contracts;
using FastProjector.Models.PropertyMetadatas;
using FastProjector.Models.TypeInformations;
using SourceCreationHelper;
using SourceCreationHelper.Interfaces;

namespace FastProjector.Models.Projections
{
    internal sealed class ClassProjection : Projection, IMapBasedProjection
    {
        private readonly CollectionTypeInformation _sourceTypeInformation;
        private readonly CollectionTypeInformation _destinationTypeInformation;

        public ClassProjection(CollectionTypeInformation sourceTypeInformation,
            CollectionTypeInformation destinationTypeInformation)
            : base(destinationTypeInformation)
        {
            _sourceTypeInformation = sourceTypeInformation;
            _destinationTypeInformation = destinationTypeInformation;
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

        public bool HasModelMap()
        {
            return ModelMap is not null;
        }

        private void ValidateMap()
        {
            if(ModelMap is null)
                return;
            if (!ModelMap.SourceType.Equals(_sourceTypeInformation.GetCollectionType()))
                throw new ArgumentException("Invalid Metadata passed");

            if (!ModelMap.DestinationType.Equals(_destinationTypeInformation.GetCollectionType()))
                throw new ArgumentException("Invalid Metadata passed");
        }
    }
}