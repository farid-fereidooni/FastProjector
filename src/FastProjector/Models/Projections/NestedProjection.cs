using System;
using FastProjector.Contracts;
using FastProjector.Models.Assignments;
using FastProjector.Models.PropertyMetadatas;
using FastProjector.Models.TypeInformations;
using FastProjector.Models.TypeMetaDatas;
using SourceCreationHelper;
using SourceCreationHelper.Interfaces;

namespace FastProjector.Models.Projections
{
    internal abstract class NestedProjection : Projection
    {
        private readonly IProjection _innerProjection;

        protected NestedProjection(IProjection innerProjection,
            CollectionTypeInformation destinationTypeInformation)
            : base(destinationTypeInformation)
        {
            _innerProjection = innerProjection;
        }

        public override ISourceText CreateProjection(IModelMapService mapService, ISourceText parameterName)
        {
            var projectionParameterName = SourceCreator.CreateSource(mapService.GetNewProjectionVariableName());

            var nestedProjectionSource = _innerProjection.CreateProjection(mapService, projectionParameterName);

            if (nestedProjectionSource is null)
                return null;

            var projectionSource = CreateSelectExpression(projectionParameterName.Text, nestedProjectionSource);

            var enumerableCastInfo = CreateIEnumerableCasting(mapService);

            if (enumerableCastInfo.IsUnMappable)
                return null;

            return SourceCreator.CreateSource(
                enumerableCastInfo.Cast($"{parameterName}.{projectionSource}"));
        }

        public new static NestedProjection Create(CollectionTypeMetaData sourceTypeMetaData,
            CollectionTypeMetaData destinationTypeMetaData)
        {
            var innerProjection = Projection.Create(sourceTypeMetaData.GetCollectionType() as CollectionTypeMetaData,
                destinationTypeMetaData.GetCollectionType() as CollectionTypeMetaData);

            if (innerProjection is IMapBasedProjection mapBasedProjection)
            {
                return new ClassNestedProjection(mapBasedProjection,
                    destinationTypeMetaData.TypeInformation);
            }

            return new PrimitiveNestedProjection(innerProjection, destinationTypeMetaData.TypeInformation);
        }
    }
}