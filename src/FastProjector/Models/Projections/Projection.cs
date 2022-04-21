using FastProjector.Contracts;
using FastProjector.Models.TypeInformations;
using SourceCreationHelper;
using SourceCreationHelper.Interfaces;

namespace FastProjector.Models.Projections
{
    internal abstract class Projection
    {
        public abstract ISourceText CreateProjection(IModelMapService mapService, ISourceText parameterName);
        
        protected ICallSourceText CreateSelectExpression(string paramName, ISourceText returnExpression)
        {
            var selectExpression = SourceCreator.CreateLambdaExpression()
                .AddParameter(paramName)
                .AssignBodyExpression(returnExpression);

            return SourceCreator.CreateCall("Select")
                .AddArgument(selectExpression);
        }

        protected TypeInformation CreateIEnumerableTypeInformation(TypeInformation genericType)
        {
            return new GenericCollectionTypeInformation(CollectionTypeEnum.System_Collections_Generic_IEnumerable_T,
                genericType);
        }
    }
    
  
}