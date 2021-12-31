using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using FastProjector.MapGenerator.Analyzing;
using FastProjector.MapGenerator.DevTools;
using FastProjector.MapGenerator.Proccessing.Models;
using FastProjector.MapGenerator.SourceGeneration.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using FastProjector.MapGenerator.SourceGeneration;

namespace FastProjector.MapGenerator.Proccessing
{
    
    internal class RequestProcessing
    {
        private readonly PropertyCasting _propertyCasting;
        private readonly MapCache _mapCache;
        private readonly SourceGenerator _sourceGenerator;

        public RequestProcessing(CastMetaData metaData)
        {
            _propertyCasting = new PropertyCasting();
            _mapCache = new MapCache();
            _sourceGenerator = new SourceGenerator();

        }
        public string ProcessProjectionRequest(IEnumerable<ProjectionRequest> requests)
        { 
            foreach(var item in requests)
            {
                CreateMappings(item.ProjectionSource, item.ProjectionTarget);
            }
            return "";
        }

        private ModelMapMetaData CreateMappings(INamedTypeSymbol sourceSymbol, INamedTypeSymbol targetSymbol, int level = 1)
        {

            var sourceType = sourceSymbol.ToTypeInformation();
            var destinationType = targetSymbol.ToTypeInformation();
            
            //check if already cached
            var cached = _mapCache.Get(sourceType, destinationType);
            if (cached != null)
                return cached;
            
            var bindingSourceCode = new List<IAssignmentSourceText>();
            var sourceProps = ExtractProps(sourceSymbol, PropertyAccessTypeEnum.HasPublicGet);
            
            var destinationProps = new HashSet<IPropertySymbol>(ExtractProps(targetSymbol, PropertyAccessTypeEnum.HasPublicSet), SymbolEqualityComparer.Default);

            
            foreach(var sourceProp in sourceProps)
            {
                var destinationProp = destinationProps.FirstOrDefault(f => f.Name == sourceProp.Name);
                if (destinationProp == null)
                    continue;
                
                var propAssignment = HandlePropertyMapping(level, destinationProp, sourceProp, bindingSourceCode);
                if (propAssignment == null)
                    continue;
                
                bindingSourceCode.Add(propAssignment);
            }

            return null;
        }

        private IAssignmentSourceText HandlePropertyMapping(int level, IPropertySymbol destinationProp, IPropertySymbol sourceProp,
            List<IAssignmentSourceText> bindingSourceCode)
        {
            
            var sourcePropMetadata = new PropertyMetaData(sourceProp);
            var destinationPropMetaData = new PropertyMetaData(destinationProp);

            var sourcePropType = sourcePropMetadata.PropertyTypeInformation;
            var destinationPropType = destinationPropMetaData.PropertyTypeInformation;

            if (sourcePropType.IsSameAs(destinationPropType))
            {
                switch (sourcePropType.TypeCategory)
                {
                    case PropertyTypeCategoryEnum.CollectionObject:
                        return Project(level, sourcePropMetadata, destinationPropMetaData);
                    case PropertyTypeCategoryEnum.CollectionPrimitive:
                        return ReCreateCollection(level, sourcePropMetadata, destinationPropMetaData);
                    case PropertyTypeCategoryEnum.SinglePrimitive:
                        return Bind(level, sourceProp.Name, destinationProp.Name);
                    case PropertyTypeCategoryEnum.SingleGenericClass:
                        return Map(level, sourceProp, destinationProp);
                    case PropertyTypeCategoryEnum.SingleNonGenenericClass:
                        return Map(level, sourceProp, destinationProp);
                }
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

            return null;
        }

        private IAssignmentSourceText ReCreateCollection(int level, PropertyMetaData sourcePropMetadata, PropertyMetaData destinationPropMetaData)
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

        private IAssignmentSourceText Project(int level, PropertyMetaData sourcePropMetadata, PropertyMetaData destinationPropMetaData)
        {
            // var collectionTypeAssignment = HandlePropertyMapping(sourcePropMetadata.GetCollectionTypeSymbol(),
            //     destinationPropMetaData.GetCollectionTypeSymbol());

            var collectionTypeMapping = CreateMappings(sourcePropMetadata.GetCollectionTypeSymbol(),
                destinationPropMetaData.GetCollectionTypeSymbol(), level + 2);

            if (collectionTypeMapping == null)
                return null;

            var selectExpression = CreateSelectExpression($"d{level + 1}", collectionTypeMapping.ModelMappingSource);

            return Bind(level, sourcePropMetadata.PropertySymbol.Name, selectExpression.Text);
        }


        private IAssignmentSourceText Map(int level, IPropertySymbol sourceProp,
            IPropertySymbol destinationProp)
        {
            var mappingResult = CreateMappings(sourceProp.Type as INamedTypeSymbol, destinationProp.Type as INamedTypeSymbol,
                level + 1);

            return _sourceGenerator.CreateAssignment(
                _sourceGenerator.CreateSource(sourceProp.Name),
                mappingResult.ModelMappingSource
            );  
        }

        private IAssignmentSourceText Bind(int level, string sourcePropName, string destinationPropName)
        {
            return _sourceGenerator.CreateAssignment(
                _sourceGenerator.CreateSource(sourcePropName),
                _sourceGenerator.CreateSource($"d{level}.{destinationPropName}")
            );
        }

        private ISourceText CreateSelectExpression(string paramName, ISourceText returnExpression)
        {
            var selectExpression = _sourceGenerator.CreateLambdaExpression()
                .AddParameter(paramName)
                .AssignBodyExpression(returnExpression);

            return _sourceGenerator.CreateCall("Select")
                .AddArgument(selectExpression);
        }
        
        public IEnumerable<IPropertySymbol> ExtractProps(INamedTypeSymbol classSymbol, PropertyAccessTypeEnum? propertyTypeToSearch = null )
        {
            var result = new List<IPropertySymbol>();
            var members = classSymbol.GetMembers();
            foreach(var member in members)
            {
                if (!(member is IPropertySymbol propSymbol)) continue;
                
                var isFiltered = false;    
                if(propertyTypeToSearch != null)
                {
                    if((propertyTypeToSearch & PropertyAccessTypeEnum.HasPublicGet) == PropertyAccessTypeEnum.HasPublicGet)
                    {
                        isFiltered = !IsPropertyPublic(propSymbol);
                    }
                    if((propertyTypeToSearch & PropertyAccessTypeEnum.HasPublicSet) == PropertyAccessTypeEnum.HasPublicSet)
                    {
                        isFiltered = !IsPropertySuitableForSet(propSymbol);
                    }
                    if(!isFiltered)
                    {
                        result.Add(propSymbol);
                    }
                }
                else {
                    result.Add(propSymbol);
                }
            }
            return result;
        }

        private static SyntaxNode GetNodeOfSymbol(ISymbol symbol)
        {
            
            var location = symbol.Locations.FirstOrDefault();
            return location != null ? location.SourceTree?.GetRoot()?.FindNode(location.SourceSpan) : null;
        }

        private static bool IsPropertyPublic(IPropertySymbol property)
        {
            var node = GetNodeOfSymbol(property);

            if (node is PropertyDeclarationSyntax propertyNode)
            {
                return propertyNode.Modifiers.Any(a => a.ValueText == "public");
            }
            return false;
                
        }

        private static bool IsPropertySuitableForSet(IPropertySymbol property)
        {
            if(IsPropertyPublic(property) && !property.IsReadOnly && property.SetMethod != null)
            {
                var node = GetNodeOfSymbol(property.SetMethod);
                if(node is AccessorDeclarationSyntax setterNode)
                {
                    return !setterNode.Modifiers.Any();
                }
            }
            return false;
        }

        private static string CreateBinding(IPropertySymbol sourceProp, IPropertySymbol destinationProp)
        {
            return null;
        }
       
        
    }    
}