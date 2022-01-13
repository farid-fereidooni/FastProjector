using System;
using System.Collections.Generic;
using System.Linq;
using FastProjector.MapGenerator.Proccessing.Contracts;
using FastProjector.MapGenerator.SourceGeneration;
using FastProjector.MapGenerator.SourceGeneration.Interfaces;
using Microsoft.CodeAnalysis;

namespace FastProjector.MapGenerator.Proccessing.Models
{
    internal class ModelMapMetaData
    {
        private readonly IMapCache _mapCache;
        private readonly IPropertyCasting _propertyCasting;
        private List<IAssignmentSourceText> _propertyAssignments;

        public ModelMapMetaData(TypeInformation sourceType,
            TypeInformation destinationType,
            ISourceText source,
            IEnumerable<PropertyMapMetaData> notMappedProps,
            int mapLevel)
        {
            SourceType = sourceType;
            DestinationType = destinationType;
            ModelMappingSource = source;
            _notMappedPropertiesMetaData = notMappedProps.ToList();
            MapLevel = mapLevel;
            IsValid = true;
        }

        public ModelMapMetaData(IMapCache mapCache, IPropertyCasting propertyCasting, INamedTypeSymbol sourceSymbol, INamedTypeSymbol targetSymbol)
        {
            _mapCache = mapCache;
            _propertyCasting = propertyCasting;
            CreateMapMetaData(sourceSymbol, targetSymbol, 1);
        }

        private ModelMapMetaData(IMapCache mapCache, IPropertyCasting propertyCasting, INamedTypeSymbol sourceSymbol, INamedTypeSymbol targetSymbol,
            int level)
        {
            _mapCache = mapCache;
            _propertyCasting = propertyCasting;
            CreateMapMetaData(sourceSymbol, targetSymbol, level);
        }
        
        public int MapLevel { get; }
        public TypeInformation SourceType { get; private set; }
        public TypeInformation DestinationType { get; private set; }
        public ISourceText ModelMappingSource { get; private set; }

        private readonly List<PropertyMapMetaData> _notMappedPropertiesMetaData;
        public IReadOnlyCollection<PropertyMapMetaData> NotMappedPropertiesMetaData => _notMappedPropertiesMetaData;
        public bool IsValid { get; set; }

        private void CreateMapMetaData(INamedTypeSymbol sourceSymbol, INamedTypeSymbol targetSymbol, int level)
        {
            SourceType = sourceSymbol.ToTypeInformation();
            DestinationType = targetSymbol.ToTypeInformation();
            
            // //check if already cached
            // var cached = mapCache.Get(sourceType, destinationType);
            // if (cached != null)
            //     return cached;
            _propertyAssignments = new List<IAssignmentSourceText>();

            if (!CheckIfMappingPossible(sourceSymbol, targetSymbol))
            {
                IsValid = true;
                return;
            }

            var propertyAssignment = new List<IAssignmentSourceText>();
            var sourceProps = sourceSymbol.ExtractProps().Where(w => w.IsPublic());

            var destinationProps = new HashSet<IPropertySymbol>(
                targetSymbol.ExtractProps().Where(w => w.IsSettable()), SymbolEqualityComparer.Default);

            foreach (var sourceProp in sourceProps)
            {
                var destinationProp = destinationProps.FirstOrDefault(f => f.Name == sourceProp.Name);
                if (destinationProp == null)
                    continue;

                HandlePropertyMapping(level, destinationProp, sourceProp, propertyAssignment);
            }

            ModelMappingSource = SourceGenerator.CreateSource("new " + SourceGenerator.CreateMemberInit(propertyAssignment).Text);
        }

        
        private void HandlePropertyMapping(int level, IPropertySymbol destinationProp, IPropertySymbol sourceProp,
            List<IAssignmentSourceText> bindingSourceCode)
        {
            var sourcePropMetadata = new PropertyMetaData(sourceProp);
            var destinationPropMetaData = new PropertyMetaData(destinationProp);

            var sourcePropType = sourcePropMetadata.PropertyTypeInformation;
            var destinationPropType = destinationPropMetaData.PropertyTypeInformation;

            if (sourcePropType.IsSameAs(destinationPropType))
            {
                AssignSamePropertyType(sourcePropType.TypeCategory, level, sourcePropMetadata, destinationPropMetaData);
            }
            else
            {
                // tryCast

                // bool castResult;
                //
                // if (castResult)
                // {
                //     //bind
                // }
                // else
                // {
                //     if (sourcePropType.IsEnumerable() && destinationPropType.IsEnumerable())
                //     {
                //         //try cast generic Type
                //         bool genericCastResult;
                //
                //         if (genericCastResult)
                //         {
                //             //project with cast
                //         }
                //         else
                //         {
                //             if (sourcePropType.IsCollectionObject() && destinationPropType.IsCollectionObject())
                //             {
                //                 //store it for later cast
                //             }
                //         }
                //     }
                // }
            }
        }
        
        private void AssignSamePropertyType(PropertyTypeCategoryEnum propertyType, int level, PropertyMetaData sourcePropMetadata, PropertyMetaData destinationPropMetadata)
        {
            var assignment = propertyType switch
            {
                PropertyTypeCategoryEnum.CollectionObject =>
                    Project(level, sourcePropMetadata, destinationPropMetadata),
                PropertyTypeCategoryEnum.CollectionPrimitive => ReCreateCollection(level, sourcePropMetadata,
                    destinationPropMetadata),
                PropertyTypeCategoryEnum.SinglePrimitive => Bind(level, sourcePropMetadata.PropertySymbol.Name,
                    destinationPropMetadata.PropertySymbol.Name),
                PropertyTypeCategoryEnum.SingleGenericClass => Map(level, sourcePropMetadata.PropertySymbol,
                    destinationPropMetadata.PropertySymbol),
                PropertyTypeCategoryEnum.SingleNonGenenericClass => Map(level, sourcePropMetadata.PropertySymbol,
                    destinationPropMetadata.PropertySymbol),
                _ => null
            };
            
            if(assignment != null)
                _propertyAssignments.Add(assignment);
        }
        private IAssignmentSourceText ReCreateCollection(int level, PropertyMetaData sourcePropMetadata,
            PropertyMetaData destinationPropMetaData)
        {
            if (!sourcePropMetadata.PropertyTypeInformation.IsEnumerable() ||
                destinationPropMetaData.PropertyTypeInformation.IsEnumerable())
                throw new ArgumentException("properties must be collection");


            var castResult = _propertyCasting.CastType(sourcePropMetadata.PropertyTypeInformation,
                destinationPropMetaData.PropertyTypeInformation);
            if (castResult.IsUnMapable)
                return null;
            var castExpression = castResult.Cast(destinationPropMetaData.PropertySymbol.Name);

            return Bind(level, sourcePropMetadata.PropertySymbol.Name, castExpression);
        }

        private IAssignmentSourceText Project(int level, PropertyMetaData sourcePropMetadata,
            PropertyMetaData destinationPropMetaData)
        {
            // var collectionTypeAssignment = HandlePropertyMapping(sourcePropMetadata.GetCollectionTypeSymbol(),
            //     destinationPropMetaData.GetCollectionTypeSymbol());

            var collectionTypeMapping = new ModelMapMetaData(_mapCache,_propertyCasting,sourcePropMetadata.GetCollectionTypeSymbol(),destinationPropMetaData.GetCollectionTypeSymbol(),level +2);

            if (!collectionTypeMapping.IsValid)
                return null;

            var selectExpression = CreateSelectExpression($"d{level + 1}", collectionTypeMapping.ModelMappingSource);

            return Bind(level, sourcePropMetadata.PropertySymbol.Name, selectExpression.Text);
        }


        private IAssignmentSourceText Map(int level, IPropertySymbol sourceProp,
            IPropertySymbol destinationProp)
        {
            var mappingResult = new ModelMapMetaData(_mapCache,_propertyCasting,sourceProp.Type as INamedTypeSymbol,
                destinationProp.Type as INamedTypeSymbol,level + 1);

            if (!mappingResult.IsValid)
                return null;
            
            return SourceGenerator.CreateAssignment(
                SourceGenerator.CreateSource(sourceProp.Name),
                mappingResult.ModelMappingSource
            );
        }

        private IAssignmentSourceText Bind(int level, string sourcePropName, string destinationPropName)
        {
            return SourceGenerator.CreateAssignment(
                SourceGenerator.CreateSource(sourcePropName),
                SourceGenerator.CreateSource($"d{level}.{destinationPropName}")
            );
        }

        private ISourceText CreateSelectExpression(string paramName, ISourceText returnExpression)
        {
            var selectExpression = SourceGenerator.CreateLambdaExpression()
                .AddParameter(paramName)
                .AssignBodyExpression(returnExpression);

            return SourceGenerator.CreateCall("Select")
                .AddArgument(selectExpression);
        }

     

        public bool Is(PropertyTypeInformation sourceType, PropertyTypeInformation destinationType)
        {
            return SourceType.IsSameAs(sourceType) &&
                   DestinationType.IsSameAs(destinationType);
        }


        private bool CheckIfMappingPossible(INamedTypeSymbol sourceSymbol, INamedTypeSymbol targetSymbol)
        {
            
            if (targetSymbol.TypeKind != TypeKind.Class && sourceSymbol.TypeKind != TypeKind.Class)
                return false;

            return true;
        }
    }
}