using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using FastProjector.Contracts;
using FastProjector.Helpers;
using FastProjector.Models.Assignments;
using FastProjector.Models.PropertyMetadatas;
using FastProjector.Models.TypeMetaDatas;
using Microsoft.CodeAnalysis;
using SourceCreationHelper;
using SourceCreationHelper.Interfaces;

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

        public ModelMap CreateModelMap(IModelMapService mapService)
        {
            var sourceProps = GetSourceProperties();

            var destinationProps = new HashSet<IPropertySymbol>(GetTargetProperties(), SymbolEqualityComparer.Default);

            var assignments = GetAssignments(sourceProps, destinationProps, mapService);

            var modelMap = new ModelMap(_sourceSymbol, _targetSymbol, assignments);

            mapService.AddToCache(modelMap);

            return modelMap;
        }

        public IEnumerable<IPropertySymbol> GetSourceProperties()
        {
            return _sourceSymbol.ExtractProps().Where(w => w.IsPublic());
        }

        public IEnumerable<IPropertySymbol> GetTargetProperties()
        {
            return _targetSymbol.ExtractProps().Where(w => w.IsSettable());
        }

        private IEnumerable<PropertyAssignment> GetAssignments(IEnumerable<IPropertySymbol> sourceProperties,
            HashSet<IPropertySymbol> destinationProperties, IModelMapService modelMapService)
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

                SetModelMapIfAssignmentIsMapBased(assignment as MapBasedPropertyAssignments,
                    sourceMetadata.TypeMetaData, destinationMetadata.TypeMetaData, modelMapService);

                yield return assignment;
            }
        }

        private void SetModelMapIfAssignmentIsMapBased(MapBasedPropertyAssignments assignment,
            TypeMetaData sourceType, TypeMetaData destinationType, IModelMapService mapService)
        {
            if (assignment is null)
                return;

            var modelMap = CreateModelMapOrFetchFromCache(sourceType, destinationType, mapService);

            if (modelMap is null)
                return;

            assignment.AddModelMap(modelMap);
        }

        private ModelMap CreateModelMapOrFetchFromCache(TypeMetaData classSource,
            TypeMetaData destinationType, IModelMapService mapService)
        {
            var modelMap = mapService.FetchFromCache(classSource.TypeInformation,
                destinationType.TypeInformation);

            if (modelMap != null)
                return modelMap;
            return new ModelMapMetaData(classSource.TypeSymbol, destinationType.TypeSymbol)
                .CreateModelMap(mapService);
        }
    }
}