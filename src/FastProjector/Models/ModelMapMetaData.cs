using System.Collections.Generic;
using System.Linq;
using FastProjector.Helpers;
using FastProjector.Models.Assignments;
using FastProjector.Models.PropertyMetadatas;
using FastProjector.Models.TypeInformations;
using Microsoft.CodeAnalysis;

namespace FastProjector.Models
{
    internal class ModelMapMetaData
    {
        private readonly ITypeSymbol _sourceSymbol;
        private readonly ITypeSymbol _targetSymbol;

        public ModelMapMetaData(ITypeSymbol sourceSymbol, ITypeSymbol targetSymbol)
        {
            _sourceSymbol = sourceSymbol;
            _targetSymbol = targetSymbol;
        }

        public TypeInformation SourceTypeInformation => _sourceSymbol.ToTypeInformation();
        public TypeInformation DestinationTypeInformation => _targetSymbol.ToTypeInformation();

        public IEnumerable<PropertyAssignment> CreateAssignments()
        {
            var sourceProps = GetSourceProperties();

            var destinationProps = new HashSet<IPropertySymbol>(GetTargetProperties(), SymbolEqualityComparer.Default);
            
            return GetAssignments(sourceProps, destinationProps).ToList();

        }
        public bool CheckIfMappingPossible()
        {
            return _targetSymbol.IsClass()
                   && _sourceSymbol.IsClass()
                   && _sourceSymbol.HasParameterlessConstructor();
        }
        private IEnumerable<IPropertySymbol> GetSourceProperties()
        {
            return _sourceSymbol.ExtractProps().Where(w => w.IsPublic());
        }

        private IEnumerable<IPropertySymbol> GetTargetProperties()
        {
            return _targetSymbol.ExtractProps().Where(w => w.IsSettable());
        }

        private IEnumerable<PropertyAssignment> GetAssignments(IEnumerable<IPropertySymbol> sourceProperties,
            HashSet<IPropertySymbol> destinationProperties)
        {
            foreach (var sourceProp in sourceProperties)
            {
                var destinationProp = destinationProperties.FirstOrDefault(f => f.Name == sourceProp.Name);
                if (destinationProp == null) continue;

                var sourceMetadata = PropertyMetadata.Create(sourceProp);

                if (sourceMetadata.TypeMetaData is null)
                    continue;

                var destinationMetadata = PropertyMetadata.Create(destinationProp);

                if (destinationMetadata.TypeMetaData is null)
                    continue;

                var assignment = PropertyAssignment.Create(sourceMetadata, destinationMetadata);

                if (assignment is null)
                    continue;

                yield return assignment;
            }
        }

    }
}