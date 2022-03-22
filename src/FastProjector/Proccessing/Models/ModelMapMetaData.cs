using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using FastProjector.MapGenerator.Proccessing.Contracts;
using FastProjector.MapGenerator.Proccessing.Models.Assignments;
using Microsoft.CodeAnalysis;
using SourceCreationHelper;
using SourceCreationHelper.Interfaces;

[assembly:InternalsVisibleTo("FastProjector.Test")]
namespace FastProjector.MapGenerator.Proccessing.Models
{
    internal class ModelMapMetaData
    {
        private readonly IMapCache _mapCache;
        private readonly ICastingService _castingService;
        private readonly PropertyMetaDataFactory _metaDataFactory;
        private readonly PropertyAssignmentFactory _assignmentFactory;
        private List<IAssignmentSourceText> _propertyAssignments;
   
        public ModelMapMetaData(IMapCache mapCache, ICastingService castingService, ITypeSymbol sourceSymbol, ITypeSymbol targetSymbol,
            PropertyMetaDataFactory metaDataFactory, PropertyAssignmentFactory assignmentFactory, int level = 1)
        {
            _mapCache = mapCache;
            _castingService = castingService;
            _metaDataFactory = metaDataFactory;
            _assignmentFactory = assignmentFactory;
            _notMappedProperties = new List<PropertyAssignment>();
            CreateMapMetaData(sourceSymbol, targetSymbol, level);
        }
        
        public int MapLevel { get; }
        public TypeInformation SourceType { get; private set; }
        public TypeInformation DestinationType { get; private set; }
        public ISourceText ModelMappingSource { get; private set; }

        private readonly List<PropertyAssignment> _notMappedProperties;
        public IReadOnlyCollection<PropertyAssignment> NotMappedProperties => _notMappedProperties;
        public bool IsValid { get; set; }

        private void CreateMapMetaData(ITypeSymbol sourceSymbol, ITypeSymbol targetSymbol, int level)
        {
            SourceType = sourceSymbol.ToTypeInformation();
            DestinationType = targetSymbol.ToTypeInformation();

            _propertyAssignments = new List<IAssignmentSourceText>();

            if (!CheckIfMappingPossible(sourceSymbol, targetSymbol))
            {
                return;
            }

            var sourceProps = sourceSymbol.ExtractProps().Where(w => w.IsPublic());

            var destinationProps = new HashSet<IPropertySymbol>(
                targetSymbol.ExtractProps().Where(w => w.IsSettable()), SymbolEqualityComparer.Default);

            foreach (var sourceProp in sourceProps)
            {
                var destinationProp = destinationProps.FirstOrDefault(f => f.Name == sourceProp.Name);
                if (destinationProp == null)
                    continue;

                HandlePropertyMapping(level, destinationProp, sourceProp);
            }

            var instantiatingExpression = $"new {DestinationType.FullName} ";
            
            ModelMappingSource = SourceCreator.CreateSource(instantiatingExpression + SourceCreator.CreateMemberInit(_propertyAssignments).Text);
            IsValid = true;
        }
        
        private void HandlePropertyMapping(int level, IPropertySymbol destinationProp, IPropertySymbol sourceProp)
        {
            var sourcePropMetadata = _metaDataFactory.CreatePropertyMetaData(sourceProp);
            var destinationPropMetaData = _metaDataFactory.CreatePropertyMetaData(destinationProp);

            var assigmentMetaData =
                _assignmentFactory.CreateAssignmentMetadata(sourcePropMetadata, destinationPropMetaData, level);

            var assignment = assigmentMetaData.CreateAssignment(_castingService);

            if (assignment != null)
            {
                _propertyAssignments.Add(assignment);
                return;
            }
            
            if(assigmentMetaData.CanMapLater())
                _notMappedProperties.Add(assigmentMetaData);
            

            // if (sourcePropType.Equals(destinationPropType))
            // {
            //     AssignSamePropertyType(sourcePropType.TypeCategory, level, sourcePropMetadata, destinationPropMetaData);
            //     return;
            // }
            //
            // // tryCast
            // var castResult = _castingService.CastType(sourcePropType, destinationPropType);
            //
            // if (!castResult.IsUnMapable)
            // {
            //     Bind(level, sourceProp.Name, castResult.Cast(destinationProp.Name));
            //     return;
            // }
            //
            // if (sourcePropType.IsEnumerable() && destinationPropType.IsEnumerable())
            // {
            //     var sourceCollectionType = new PropertyTypeInformation(sourcePropMetadata.GetCollectionTypeSymbol());
            //     var destinationCollectionType = new PropertyTypeInformation(destinationPropMetaData.GetCollectionTypeSymbol());
            //
            //     var castGenericTypes = _castingService.CastType(sourceCollectionType, destinationCollectionType);
            //     if (!castGenericTypes.IsUnMapable)
            //         Project(level, sourcePropMetadata, sourcePropMetadata, castGenericTypes.Cast);
            //     return;
            //
            // }
            // if (sourcePropType.IsCollectionObject() && destinationPropType.IsCollectionObject())
            // {
            //     _notMappedProperties.Add(new PropertyMapMetaData(sourcePropMetadata , destinationPropMetaData));
            // }
            //
            // if (sourcePropType.IsNonGenericClass() && destinationPropType.IsNonGenericClass())
            // {
            //     _notMappedProperties.Add(new PropertyMapMetaData(sourcePropMetadata , destinationPropMetaData));
            // }
            
        }
        
        // private void AssignSamePropertyType(PropertyTypeCategoryEnum propertyType, int level, PropertyMetaData sourcePropMetadata, PropertyMetaData destinationPropMetadata)
        // {
        //     switch (propertyType)
        //     {
        //         case PropertyTypeCategoryEnum.CollectionObject:
        //             Project(level, sourcePropMetadata, destinationPropMetadata);
        //             break;
        //         case PropertyTypeCategoryEnum.CollectionPrimitive:
        //             ReCreateCollection(level, sourcePropMetadata, destinationPropMetadata);
        //             break;
        //         case PropertyTypeCategoryEnum.SinglePrimitive:
        //             Bind(level, sourcePropMetadata.PropertySymbol.Name, destinationPropMetadata.PropertySymbol.Name);
        //             break;
        //         case PropertyTypeCategoryEnum.SingleGenericClass:
        //             Map(level, sourcePropMetadata.PropertySymbol, destinationPropMetadata.PropertySymbol);
        //             break;
        //         case PropertyTypeCategoryEnum.SingleNonGenenericClass:
        //             Map(level, sourcePropMetadata.PropertySymbol, destinationPropMetadata.PropertySymbol);
        //             break;
        //         case PropertyTypeCategoryEnum.Unknown:
        //         default:
        //             throw new ArgumentOutOfRangeException(nameof(propertyType), propertyType, null);
        //     }
        // }
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
        // private void Map(int level, IPropertySymbol sourceProp,
        //     IPropertySymbol destinationProp)
        // {
        //     var mappingResult = CreateOrFetchFromCache(sourceProp.Type as INamedTypeSymbol,
        //         destinationProp.Type as INamedTypeSymbol, level + 1);
        //
        //     if (!mappingResult.IsValid)
        //         return;
        //     _propertyAssignments.Add(
        //         SourceCreator.CreateAssignment(
        //             SourceCreator.CreateSource(sourceProp.Name),
        //             mappingResult.ModelMappingSource
        //         ));
        // }
        //
        // private void Bind(int level, string sourcePropName, string destinationPropName)
        // {
        //     _propertyAssignments.Add(
        //         SourceCreator.CreateAssignment(
        //             SourceCreator.CreateSource(sourcePropName),
        //             SourceCreator.CreateSource($"d{level}.{destinationPropName}")
        //         ));
        // }

        private ISourceText CreateSelectExpression(string paramName, ISourceText returnExpression)
        {
            var selectExpression = SourceCreator.CreateLambdaExpression()
                .AddParameter(paramName)
                .AssignBodyExpression(returnExpression);

            return SourceCreator.CreateCall("Select")
                .AddArgument(selectExpression);
        }

        private bool CheckIfMappingPossible(ITypeSymbol sourceSymbol, ITypeSymbol targetSymbol)
        {
            return targetSymbol.IsClass()
                   && sourceSymbol.IsClass()
                   && targetSymbol.HasParameterlessConstructor();
        }
        
        // private ModelMapMetaData CreateOrFetchFromCache(ITypeSymbol sourceType, ITypeSymbol destinationType, int level)
        // {
        //      return  _mapCache.Get(sourceType.ToTypeInformation(), destinationType.ToTypeInformation()) ??
        //              new ModelMapMetaData(_mapCache, _castingService, sourceType, destinationType);
        // }
        
    }
}