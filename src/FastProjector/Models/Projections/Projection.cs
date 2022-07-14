using FastProjector.Contracts;
using FastProjector.Models.Casting;
using FastProjector.Models.TypeInformations;
using FastProjector.Models.TypeMetaDatas;
using SourceCreationHelper;
using SourceCreationHelper.Interfaces;

namespace FastProjector.Models.Projections
{
    internal abstract class Projection : IProjection
    {
        private readonly CollectionTypeInformation _destinationTypeInformation;

        protected Projection(CollectionTypeInformation destinationTypeInformation)
        {
            _destinationTypeInformation = destinationTypeInformation;
        }

        public abstract ISourceText CreateProjection(IModelMapService mapService, ISourceText parameterName);

        protected static ICallSourceText CreateSelectExpression(string paramName, ISourceText returnExpression)
        {
            var selectExpression = SourceCreator.CreateLambdaExpression()
                .AddParameter(paramName)
                .AssignBodyExpression(returnExpression);

            return SourceCreator.CreateCall("Select")
                .AddArgument(selectExpression);
        }

        protected PropertyCastResult CreateIEnumerableCasting(IModelMapService mapService)
        {
            var iEnumerableTypeOfProjected =
                CreateIEnumerableTypeInformation(_destinationTypeInformation.GetCollectionType());

            var enumerableCastInfo = mapService.CastType(iEnumerableTypeOfProjected,
                _destinationTypeInformation);

            return enumerableCastInfo;
        }

        private static TypeInformation CreateIEnumerableTypeInformation(TypeInformation genericType)
        {
            return new GenericCollectionTypeInformation(CollectionTypeEnum.System_Collections_Generic_IEnumerable_T,
                genericType);
        }

        public static Projection Create(CollectionTypeMetaData sourceTypeMetaData,
            CollectionTypeMetaData destinationTypeMetadata)
        {
            var sourceTypeInformation = sourceTypeMetaData.TypeInformation;
            var destinationTypeInformation = destinationTypeMetadata.TypeInformation;
            
            if (!sourceTypeInformation.GetCollectionType()
                    .HasSameCategory(destinationTypeInformation.GetCollectionType()))
            {
                return null;
            }

            var collectionType = sourceTypeInformation.GetCollectionType();
            
            return collectionType switch
            {
                GenericClassTypeInformation => new ClassProjection(sourceTypeMetaData, destinationTypeMetadata),
                ClassTypeInformation => new ClassProjection(sourceTypeMetaData, destinationTypeMetadata),
                CollectionTypeInformation => NestedProjection.Create(sourceTypeMetaData, destinationTypeMetadata),
                PrimitiveTypeInformation => new PrimitiveProjection(sourceTypeInformation, destinationTypeInformation),
                _ => null
            };
        }
    }
}