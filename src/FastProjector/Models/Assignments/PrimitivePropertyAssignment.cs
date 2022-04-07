using FastProjector.Contracts;
using FastProjector.Models.PropertyMetaDatas;
using SourceCreationHelper;
using SourceCreationHelper.Interfaces;

namespace FastProjector.Models.Assignments
{
    internal class PrimitivePropertyAssignment: PropertyAssignment
    {
        private readonly PrimitivePropertyMetaData _sourceProperty;
        private readonly PrimitivePropertyMetaData _destinationProperty;

        public PrimitivePropertyAssignment(PrimitivePropertyMetaData sourceProperty, PrimitivePropertyMetaData destinationProperty)
        {
            _sourceProperty = sourceProperty;
            _destinationProperty = destinationProperty;
        }

        public override IAssignmentSourceText CreateAssignmentSource(IModelMapService mapService, ISourceText parameterName)
        {
            if (_sourceProperty.Equals(_destinationProperty))
            {
                return SourceCreator.CreateAssignment(
                    SourceCreator.CreateSource(_sourceProperty.GetPropertyName()),
                    SourceCreator.CreateSource($"{parameterName}.{_destinationProperty.GetPropertyName()}")
                );
            }
            
            // try cast
            var castResult = mapService.CastType(_sourceProperty.PropertyTypeInformation,
                                                     _destinationProperty.PropertyTypeInformation);
            
            if(!castResult.IsUnMapable)
                return SourceCreator.CreateAssignment(
                    SourceCreator.CreateSource(_destinationProperty.GetPropertyName()),
                    SourceCreator.CreateSource(castResult.Cast($"{parameterName}.{_sourceProperty.GetPropertyName()}"))
                );

            return null;

        }

        public override bool CanMapLater() => false;
    }
}