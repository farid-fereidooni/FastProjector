using System;
using FastProjector.Contracts;
using FastProjector.Models.Projections;
using FastProjector.Models.PropertyMetadatas;
using FastProjector.Models.TypeMetaDatas;
using SourceCreationHelper;
using SourceCreationHelper.Interfaces;

namespace FastProjector.Models.Assignments
{
    internal class PrimitiveCollectionPropertyAssignment : PropertyAssignment
    {
        private readonly IProjection _projection;
        private readonly CollectionPropertyMetadata _destinationMetaData;

        public PrimitiveCollectionPropertyAssignment(IProjection projection,
            CollectionPropertyMetadata destinationMetaData)
        {
            _projection = projection;
            _destinationMetaData = destinationMetaData;
        }

        public override IAssignmentSourceText CreateAssignmentSource(IModelMapService mapService,
            ISourceText parameterName)
        {
            var rightPartOfAssignmentParameterName =
                SourceCreator.CreateSource($"{parameterName}.{_destinationMetaData.PropertyName}");

            var projection = _projection.CreateProjection(mapService, rightPartOfAssignmentParameterName);

            if (projection is null)
            {
                return null;
            }

            return SourceCreator.CreateAssignment(
                SourceCreator.CreateSource(_destinationMetaData.PropertyName),
                projection
            );
        }
    }
}