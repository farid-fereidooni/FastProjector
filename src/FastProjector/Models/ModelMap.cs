using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using FastProjector.Contracts;
using FastProjector.Helpers;
using FastProjector.Models.Assignments;
using FastProjector.Models.TypeInformations;
using Microsoft.CodeAnalysis;
using SourceCreationHelper;
using SourceCreationHelper.Interfaces;

[assembly: InternalsVisibleTo("FastProjector.Test")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace FastProjector.Models
{
    internal class ModelMap
    {
        private readonly ModelMapMetaData _modelMapMetaData;
        private readonly IEnumerable<PropertyAssignment> _propertyAssignments;

        public ModelMap(ModelMapMetaData modelMapMetaData)
        {
            _modelMapMetaData = modelMapMetaData;
            SourceType = _modelMapMetaData.SourceTypeInformation;
            DestinationType = _modelMapMetaData.DestinationTypeInformation;
            _propertyAssignments = modelMapMetaData.CreateAssignments();
        }

        public TypeInformation SourceType { get; }
        public TypeInformation DestinationType { get; }

        public ISourceText CreateMappingSource(IModelMapService mapService, ISourceText parameterName)
        {
            if (!_modelMapMetaData.CheckIfMappingPossible())
            {
                throw new InvalidOperationException("Mapping was not possible for the requested types");
            }

            var assignmentSources = _propertyAssignments
                .Select(x => x.CreateAssignmentSource(mapService, parameterName))
                .Where(w => w is not null);

            var instantiatingExpression = $"new {DestinationType.FullName} ";

            return SourceCreator.CreateSource(instantiatingExpression +
                                              SourceCreator.CreateMemberInit(assignmentSources).Text);
        }

        public bool RequiresModelMaps()
        {
            return _propertyAssignments.Any(assignment =>
                assignment is MapBasedPropertyAssignments mapBasedPropertyAssignment &&
                !mapBasedPropertyAssignment.HasModelMap());
        }

        public void TryResolveRequiredMaps(IMapResolverService mapResolverService)
        {
            var mapLessAssignments = _propertyAssignments.Where(assignment =>
                assignment is MapBasedPropertyAssignments mapBasedPropertyAssignment &&
                !mapBasedPropertyAssignment.HasModelMap())
                .Cast<MapBasedPropertyAssignments>();

            foreach (var mapBasedPropertyAssignments in mapLessAssignments)
            {
                var requiredMapTypes = mapBasedPropertyAssignments.GetRequiredMapTypes();
                var requiredMap =
                    mapResolverService.ResolveMap(requiredMapTypes.sourceType, requiredMapTypes.destinationType);

                if (requiredMap is null) continue;
                
                mapBasedPropertyAssignments.AddModelMap(requiredMap);
            }
        }

      
    }
}