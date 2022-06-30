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

[assembly:InternalsVisibleTo("FastProjector.Test")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace FastProjector.Models
{
    internal class ModelMap
    {
        private readonly ITypeSymbol _sourceSymbol;
        private readonly ITypeSymbol _targetSymbol;
        private readonly IEnumerable<PropertyAssignment> _propertyAssignments;
   
        public ModelMap(ITypeSymbol sourceSymbol, ITypeSymbol targetSymbol, IEnumerable<PropertyAssignment> assignments)
        {   
            _sourceSymbol = sourceSymbol;
            _targetSymbol = targetSymbol;
            SourceType = _sourceSymbol.ToTypeInformation();
            DestinationType = _targetSymbol.ToTypeInformation();
            _propertyAssignments = assignments;
            _notMappedProperties = new List<PropertyAssignment>();
        }
        
        public TypeInformation SourceType { get; }
        public TypeInformation DestinationType { get; }

        private readonly List<PropertyAssignment> _notMappedProperties;
        public IReadOnlyCollection<PropertyAssignment> NotMappedProperties => _notMappedProperties;


        public ISourceText CreateMappingSource(IModelMapService mapService, ISourceText parameterName)
        {
            if (!CheckIfMappingPossible())
            {
                throw new InvalidOperationException("Mapping was not possible for the requested types");
            }

            var assignmentSources = _propertyAssignments
                .Select(x => x.CreateAssignmentSource(mapService, parameterName))
                .Where(w => w is not null);
   
            var instantiatingExpression = $"new {DestinationType.FullName} ";
            
            return SourceCreator.CreateSource(instantiatingExpression + SourceCreator.CreateMemberInit(assignmentSources).Text);
        }

        public bool CheckIfMappingPossible()
        {
            return _targetSymbol.IsClass()
                   && _sourceSymbol.IsClass()
                   && _sourceSymbol.HasParameterlessConstructor();
  
        }
    }
}