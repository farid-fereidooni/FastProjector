using System;
using FastProjector.Contracts;
using FastProjector.Models.Projections;
using FastProjector.Models.PropertyMetadatas;
using FastProjector.Models.TypeMetaDatas;
using SourceCreationHelper;
using SourceCreationHelper.Interfaces;

namespace FastProjector.Models.Assignments
{
    internal class ClassCollectionPropertyAssignment : MapBasedPropertyAssignments
    {
        private readonly IMapBasedProjection _projection;
        private readonly CollectionPropertyMetadata _destinationType;

        public ClassCollectionPropertyAssignment(IMapBasedProjection projection,
            CollectionPropertyMetadata destinationType)
        {
            if (destinationType.TypeMetaData.GetCollectionType() is not ClassTypeMetaData)
            {
                throw new ArgumentException($"{nameof(destinationType)} is not valid type");
            }

            _projection = projection;
            _destinationType = destinationType;
        }

        public override IAssignmentSourceText CreateAssignmentSource(IModelMapService mapService,
            ISourceText parameterName)
        {
            var rightPartOfAssignmentParameterName =
                SourceCreator.CreateSource($"{parameterName}.{_destinationType.PropertyName}");

            var projection = _projection.CreateProjection(mapService, rightPartOfAssignmentParameterName);

            if (projection is null)
            {
                return null;
            }

            return SourceCreator.CreateAssignment(
                SourceCreator.CreateSource(_destinationType.PropertyName),
                projection
            );
        }

        public override (TypeMetaData sourceType, TypeMetaData destinationType) GetRequiredMapTypes()
        {
            return _projection.GetRequiredMapTypes();
        }

        public override void AddModelMap(ModelMap modelMap)
        {
            base.AddModelMap(modelMap);
            _projection.AddModelMap(modelMap);
        }
    }
}