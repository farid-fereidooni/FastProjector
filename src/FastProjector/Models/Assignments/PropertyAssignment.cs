using FastProjector.Contracts;
using FastProjector.Models.Projections;
using FastProjector.Models.PropertyMetadatas;
using SourceCreationHelper.Interfaces;

namespace FastProjector.Models.Assignments
{
    internal abstract class PropertyAssignment
    {
        public abstract IAssignmentSourceText CreateAssignmentSource(IModelMapService mapService,
            ISourceText parameterName);

        public static PropertyAssignment Create(PropertyMetadata sourceType,
            PropertyMetadata destinationType)
        {
            if (!sourceType.TypeMetaData.HasSameTypeCategory(destinationType.TypeMetaData))
            {
                return null;
            }

            return sourceType switch
            {
                PrimitivePropertyMetadata primitiveSource => new PrimitivePropertyAssignment(primitiveSource,
                    destinationType as PrimitivePropertyMetadata),

                ClassPropertyMetadata classSource => new ClassPropertyAssignment(classSource,
                    destinationType as ClassPropertyMetadata),

                CollectionPropertyMetadata collectionType => CreateCollection(collectionType,
                    (CollectionPropertyMetadata) destinationType),

                _ => null
            };
        }

        private static PropertyAssignment CreateCollection(CollectionPropertyMetadata sourceType,
            CollectionPropertyMetadata destinationType)
        {
            var projection = Projection.Create(sourceType.TypeMetaData,
                destinationType.TypeMetaData);

            if (projection is null)
                return null;

            return projection switch
            {
                ClassProjection classProjection => new ClassCollectionPropertyAssignment(classProjection,
                    destinationType),
                ClassNestedProjection classNestedProjection => new ClassCollectionPropertyAssignment(
                    classNestedProjection,
                    destinationType),
                PrimitiveNestedProjection primitiveNestedProjection => new PrimitiveCollectionPropertyAssignment(
                    primitiveNestedProjection, destinationType),
                PrimitiveProjection primitiveProjection => new PrimitiveCollectionPropertyAssignment(
                    primitiveProjection, destinationType),
                _ => null
            };
        }
    }
}