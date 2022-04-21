using FastProjector.Contracts;
using FastProjector.Models.PropertyMetadatas;
using FastProjector.Models.TypeMetaDatas;
using SourceCreationHelper;
using SourceCreationHelper.Interfaces;

namespace FastProjector.Models.Assignments
{
    internal class PrimitivePropertyAssignment: PropertyAssignment
    {
        private readonly PrimitivePropertyMetadata _sourceType;
        private readonly PrimitivePropertyMetadata _destinationType;

        public PrimitivePropertyAssignment(PrimitivePropertyMetadata sourceType, PrimitivePropertyMetadata destinationType)
        {
            _sourceType = sourceType;
            _destinationType = destinationType;
        }

        public override IAssignmentSourceText CreateAssignmentSource(IModelMapService mapService, ISourceText parameterName)
        {
            if (_sourceType.Equals(_destinationType))
            {
                return SourceCreator.CreateAssignment(
                    SourceCreator.CreateSource(_sourceType.PropertyName),
                    SourceCreator.CreateSource($"{parameterName}.{_destinationType.PropertyName}")
                );
            }
            
            // try cast
            var castResult = mapService.CastType(_sourceType.TypeMetaData.TypeInformation,
                                                     _destinationType.TypeMetaData.TypeInformation);
            
            if(!castResult.IsUnMapable)
                return SourceCreator.CreateAssignment(
                    SourceCreator.CreateSource(_destinationType.PropertyName),
                    SourceCreator.CreateSource(castResult.Cast($"{parameterName}.{_sourceType.PropertyName}"))
                );

            return null;

        }

        public override bool CanMapLater() => false;
    }
}