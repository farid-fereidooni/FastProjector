using System;
using FastProjector.Contracts;
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

            var projectionSource = CreateSelectExpression(projectionParameterName.Text, nestedProjectionSource);

            var enumerableCastInfo = CreateIEnumerableCasting(mapService);

            if (enumerableCastInfo.IsUnMapable)
                return null;

            return SourceCreator.CreateSource(
                enumerableCastInfo.Cast($"{parameterName}.{projectionSource}"));
            
        }

        public new static NestedProjection Create(CollectionTypeMetaData sourceTypeMetaData,
            CollectionTypeMetaData destinationTypeMetaData)
        {
            var innerProjection = Projection.Create(sourceTypeMetaData, destinationTypeMetaData);

            if (innerProjection.GetType().IsSubclassOf(typeof(IMapBasedProjection)))
            {
                return new ClassNestedProjection(innerProjection as IMapBasedProjection, destinationTypeMetaData.TypeInformation);
            }
            return new PrimitiveNestedProjection(innerProjection, destinationTypeMetaData.TypeInformation);
        }

    }
}