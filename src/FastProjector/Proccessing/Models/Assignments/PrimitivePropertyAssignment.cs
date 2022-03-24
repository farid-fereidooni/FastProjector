using FastProjector.MapGenerator.Proccessing.Contracts;
using SourceCreationHelper;
using SourceCreationHelper.Interfaces;

namespace FastProjector.MapGenerator.Proccessing.Models.Assignments
{
    internal class PrimitivePropertyAssignment: PropertyAssignment
    {
        private readonly PrimitivePropertyMetaData _sourceProperty;
        private readonly PrimitivePropertyMetaData _destinationProperty;
        private readonly ICastingService _castingService;
        private readonly int _level;

        public PrimitivePropertyAssignment(PrimitivePropertyMetaData sourceProperty, PrimitivePropertyMetaData destinationProperty,ICastingService castingService, int level)
        {
            _sourceProperty = sourceProperty;
            _destinationProperty = destinationProperty;
            _castingService = castingService;
            _level = level;
        }

        public override IAssignmentSourceText CreateAssignment()
        {

            if (_sourceProperty.Equals(_destinationProperty))
            {
                return SourceCreator.CreateAssignment(
                    SourceCreator.CreateSource(_sourceProperty.GetPropertyName()),
                    SourceCreator.CreateSource($"d{_level}.{_destinationProperty.GetPropertyName()}")
                );
            }
            
            // try cast
            var castResult = _castingService.CastType(_sourceProperty.PropertyTypeInformation,
                                                     _destinationProperty.PropertyTypeInformation);
            
            if(!castResult.IsUnMapable)
                return SourceCreator.CreateAssignment(
                    SourceCreator.CreateSource(_destinationProperty.GetPropertyName()),
                    SourceCreator.CreateSource(castResult.Cast($"d{_level}.{_sourceProperty.GetPropertyName()}"))
                );

            return null;

        }

        public override bool CanMapLater() => false;
    }
}