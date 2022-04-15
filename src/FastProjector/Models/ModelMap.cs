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
namespace FastProjector.Models
{
    internal class ModelMap
    {
        private readonly ITypeSymbol _sourceSymbol;
        private readonly ITypeSymbol _targetSymbol;
        private IEnumerable<PropertyAssignment> _propertyAssignments;
   
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
        
        // private void ReCreateCollection(int level, PropertyMetaData sourcePropMetadata,
        //     PropertyMetaData destinationPropMetaData)
        // {
        //     if (!sourcePropMetadata.PropertyTypeInformation.IsEnumerable() ||
        //         destinationPropMetaData.PropertyTypeInformation.IsEnumerable())
        //         throw new ArgumentException("properties must be collection");
        //
        //
        //     var castResult = _castingService.CastType(sourcePropMetadata.PropertyTypeInformation,
        //         destinationPropMetaData.PropertyTypeInformation);
        //     if (castResult.IsUnMapable)
        //         return;
        //     var castExpression = castResult.Cast(destinationPropMetaData.PropertySymbol.Name);
        //
        //     Bind(level, sourcePropMetadata.PropertySymbol.Name, castExpression);
        // }

        // private void Project(int level, PropertyMetaData sourcePropMetadata,
        //     PropertyMetaData destinationPropMetaData, Func<string, string> cast = null)
        // {
        //     var collectionTypeMapping = CreateOrFetchFromCache(sourcePropMetadata.GetCollectionTypeSymbol(),
        //         destinationPropMetaData.GetCollectionTypeSymbol(),
        //         level + 2);
        //     
        //     if (!collectionTypeMapping.IsValid)
        //         return;
        //
        //     var mappingSource = cast != null
        //         ? SourceCreator.CreateSource(cast(collectionTypeMapping.ModelMappingSource.Text))
        //         : collectionTypeMapping.ModelMappingSource;
        //
        //     var selectExpression = CreateSelectExpression($"d{level + 1}", mappingSource);
        //
        //     Bind(level, sourcePropMetadata.PropertySymbol.Name, selectExpression.Text);
        // }
        //
        //


        private ISourceText CreateSelectExpression(string paramName, ISourceText returnExpression)
        {
            var selectExpression = SourceCreator.CreateLambdaExpression()
                .AddParameter(paramName)
                .AssignBodyExpression(returnExpression);

            return SourceCreator.CreateCall("Select")
                .AddArgument(selectExpression);
        }
        
        public bool CheckIfMappingPossible()
        {
            return _targetSymbol.IsClass()
                   && _sourceSymbol.IsClass()
                   && _sourceSymbol.HasParameterlessConstructor();
        }

   
    }
}