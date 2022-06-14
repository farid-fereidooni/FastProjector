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

        public ClassProjection(CollectionPropertyMetadata sourcePropertyMetadata,
            CollectionPropertyMetadata destinationPropertyMetadata)
            : base(destinationPropertyMetadata.TypeMetaData.TypeInformation as CollectionTypeInformation,
                sourcePropertyMetadata.TypeMetaData.TypeInformation as CollectionTypeInformation)
        { }

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
    }
}