using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using FastProjector.Contracts;
using FastProjector.Helpers;
using FastProjector.Models.Assignments;
using FastProjector.Models.PropertyMetaDatas;
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

        public ModelMap CreateModelMap(IModelMapService mapService) {
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

                var sourceMetadata = PropertyMetaData.Create(sourceProp);
                
                if(sourceMetadata is null)
                    continue;
                
                var destinationMetadata = PropertyMetaData.Create(destinationProp);
                
                if(destinationMetadata is null)
                    continue;
                
                var assignment = PropertyAssignment.Create(sourceMetadata, destinationMetadata);
                
                if(assignment is null)
                    continue;

                SetModelMapIfAssignmentIsMapBased(assignment as MapBasedPropertyAssignments, sourceMetadata, destinationMetadata, modelMapService);
                
                yield return assignment;
                
            }
        }

        private void SetModelMapIfAssignmentIsMapBased(MapBasedPropertyAssignments assignment,
            PropertyMetaData sourceProperty, PropertyMetaData destinationProperty, IModelMapService mapService)
        {
            if(assignment is null)
                return; 
            
            var modelMap = CreateModelMapOrFetchFromCache(sourceProperty, destinationProperty, mapService);
            
            if(modelMap is null)
                return;
            
            assignment.AddModelMap(modelMap);
            
        }
        
        private ModelMap CreateModelMapOrFetchFromCache(PropertyMetaData classSource,
            PropertyMetaData destinationProperty, IModelMapService mapService)
        {
            var modelMap = mapService.FetchFromCache(classSource.PropertyTypeInformation,
                destinationProperty.PropertyTypeInformation);
            
            if (modelMap != null)
                return modelMap;
        
            return new ModelMapMetaData(classSource.PropertySymbol.Type, destinationProperty.PropertySymbol.Type)
                .CreateModelMap(mapService);
        }
    }
}