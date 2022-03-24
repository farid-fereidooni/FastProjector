using FastProjector.Contracts;
using FastProjector.Models.PropertyMetaDatas;
using Microsoft.CodeAnalysis;
using SourceCreationHelper;
using SourceCreationHelper.Interfaces;

namespace FastProjector.Models.Assignments
{
    internal class ClassPropertyAssignment: PropertyAssignment
    {
        private readonly ClassPropertyMetaData _sourceProperty;
        private readonly ClassPropertyMetaData _destinationProperty;
        private readonly int _level;

        public ClassPropertyAssignment(ClassPropertyMetaData sourceProperty, ClassPropertyMetaData destinationProperty, int level)
        {
            _sourceProperty = sourceProperty;
            _destinationProperty = destinationProperty;
            _level = level;
        }
        public override IAssignmentSourceText CreateAssignment(IModelMapService mapService)
        {
            var cache = mapService.FetchFromCache(_sourceProperty.PropertyTypeInformation,
                _destinationProperty.PropertyTypeInformation);

            if (cache is not null)
                return cache.CreateMappingSource(mapService);
            
            var mappingResult = CreateOrFetchFromCache(sourceProp.Type as INamedTypeSymbol,
                destinationProp.Type as INamedTypeSymbol, level + 1);
            
            if (!mappingResult.IsValid)
                return;
            _propertyAssignments.Add(
                SourceCreator.CreateAssignment(
                    SourceCreator.CreateSource(sourceProp.Name),
                    mappingResult.ModelMappingSource
                ));
        }

        private IAssignmentSourceText CreateAssignment(ISourceText mapSourceText)
        {
            return SourceCreator.CreateAssignment(
                SourceCreator.CreateSource(_sourceProperty.GetPropertyName()),
                SourceCreator.CreateSource($"d{_level}.{_destinationProperty.GetPropertyName()}")
            );
        }

        public override bool CanMapLater()
        {
            return false;
        }
    }
}