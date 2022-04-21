using System;
using FastProjector.Contracts;
using FastProjector.Models.PropertyMetadatas;
using FastProjector.Models.TypeInformations;
using SourceCreationHelper;
using SourceCreationHelper.Interfaces;

namespace FastProjector.Models.Projections
{
    internal sealed class ClassProjection : MapBasedProjection
    {
        private readonly CollectionPropertyMetadata _sourcePropertyMetadata;
        private readonly CollectionPropertyMetadata _destinationPropertyMetadata;

        public ClassProjection(CollectionPropertyMetadata sourcePropertyMetadata,
            CollectionPropertyMetadata destinationPropertyMetadata, ModelMap modelMap)
            : base(modelMap)
        {
            _sourcePropertyMetadata = sourcePropertyMetadata;
            _destinationPropertyMetadata = destinationPropertyMetadata;
        }


        public override ISourceText CreateProjection(IModelMapService mapService, ISourceText parameterName)
        {
            if (ModelMap is null)
                throw new InvalidOperationException("No model map has been passed");
            var projectionParam = SourceCreator.CreateSource(mapService.GetNewProjectionVariableName());

            var modelMapSource = ModelMap.CreateMappingSource(mapService, projectionParam);
            var projectionSource = CreateSelectExpression(projectionParam.Text, modelMapSource);

            return CreateIEnumerableCasting(mapService, parameterName, projectionSource);
        }

        private ISourceText CreateIEnumerableCasting(IModelMapService mapService, ISourceText parameterName,
            ICallSourceText projectionSource)
        {
            var iEnumerableTypeOfProjected = CreateIEnumerableTypeInformation(ModelMap.DestinationType);

            var enumerableCastInfo = mapService.CastType(iEnumerableTypeOfProjected,
                _destinationPropertyMetadata.TypeMetaData.TypeInformation);

            if (enumerableCastInfo.IsUnMapable)
                return null;

            return SourceCreator.CreateSource(
                enumerableCastInfo.Cast($"{parameterName}.{_sourcePropertyMetadata.PropertyName}.{projectionSource}"));
        }

        protected override void ValidateMap()
        {
            throw new System.NotImplementedException();
        }
    }
}