using System.Collections.Generic;
using System.Linq;
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

        public RequestProcessing(CastMetaData metaData)
        {
            _propertyCasting = new PropertyCasting();
            _mapCache = new MapCache();
        }
        public string ProcessProjectionRequest(IEnumerable<ProjectionRequest> requests)
        { 
            foreach(var item in requests)
            {
                CreateMappings(item.ProjectionSource, item.ProjectionTarget);
            }
            return "";
        }

        private ModelMapMetaData CreateMappings(INamedTypeSymbol source, INamedTypeSymbol target, int level = 1)
        {

            var sourceType = source.ToTypeInformation();
            var destinationType = target.ToTypeInformation();
            
            //check if already cached
            var cached = _mapCache.Get(sourceType, destinationType);
            if (cached != null)
                return cached;
            
            var sourceGenerator = new SourceGenerator();
            var bindingSourceCode = new List<IAssignmentSourceText>();
            var sourceProps = ExtractProps(source, PropertyAccessTypeEnum.HasPublicGet);
            
            var destinationProps = new HashSet<IPropertySymbol>(ExtractProps(target, PropertyAccessTypeEnum.HasPublicSet), SymbolEqualityComparer.Default);

            
            foreach(var sourceProp in sourceProps)
            {
                var destinationProp = destinationProps.FirstOrDefault(f => f.Name == sourceProp.Name);
                if(destinationProp != null)
                {
                    var sourcePropType = new PropertyTypeInformation(sourceProp);
                    var destinationPropType = new PropertyTypeInformation(destinationProp);
                    
                    if(sourcePropType.IsSameAs(destinationPropType))
                    {
                        if (sourcePropType.TypeCategory == PropertyTypeCategoryEnum.SinglePrimitive)
                            bindingSourceCode.Add(sourceGenerator.CreateAssignment(
                                sourceGenerator.CreateSource(sourceProp.Name),
                                sourceGenerator.CreateSource(destinationProp.Name)
                            ));
                    }
                    else
                    {
                        // try cast
                        
                    }
                    // ==================================


                    if (sourcePropType.IsSameAs(destinationPropType))
                    {
                        if (sourcePropType.TypeCategory == PropertyTypeCategoryEnum.CollectionObject)
                        {
                            //project with map
                        }
                        else if(sourcePropType.TypeCategory == PropertyTypeCategoryEnum.CollectionPrimitive)
                        {
                            Bind(level, bindingSourceCode, sourceGenerator, sourceProp, destinationProp);
                        }
                        else if (sourcePropType.TypeCategory == PropertyTypeCategoryEnum.SinglePrimitive)
                        {
                            Bind(level, bindingSourceCode, sourceGenerator, sourceProp, destinationProp);
                        }
                        else if (sourcePropType.TypeCategory == PropertyTypeCategoryEnum.SingleGenericClass)
                        {
                            Map(level, bindingSourceCode, sourceGenerator, sourceProp, destinationProp);
                        }
                        else if (sourcePropType.TypeCategory == PropertyTypeCategoryEnum.SingleNonGenenericClass)
                        {
                            Map(level, bindingSourceCode, sourceGenerator, sourceProp, destinationProp);
                        }
                        
                    }
                    else
                    {
                        // tryCast

                        bool castResult ;

                        if (castResult)
                        {
                            //bind
                        }
                        else
                        {
                            if (sourcePropType.IsEnumerable() && destinationPropType.IsEnumerable())
                            {
                                //try cast generic Type
                                bool genericCastResult;
                                
                                if (genericCastResult)
                                {
                                    //project with cast
                                }
                                else
                                {
                                    if (sourcePropType.IsCollectionObject() && destinationPropType.IsCollectionObject())
                                    {
                                        //store it for later cast
                                    }
                                }
                            }
                            
                        }
                    }
                }
            }

            return null;
        }

        private void Map(int level, List<IAssignmentSourceText> bindingSourceCode, SourceGenerator sourceGenerator, IPropertySymbol sourceProp,
            IPropertySymbol destinationProp)
        {
            var mappingResult = CreateMappings(sourceProp.Type as INamedTypeSymbol, destinationProp.Type as INamedTypeSymbol,
                level + 1);

            bindingSourceCode.Add(sourceGenerator.CreateAssignment(
                sourceGenerator.CreateSource(sourceProp.Name),
                mappingResult.ModelMappingSource
            ));
        }

        private static void Bind(int level, ICollection<IAssignmentSourceText> bindingSourceCode, SourceGenerator sourceGenerator, ISymbol sourceProp,
            ISymbol destinationProp)
        {
            bindingSourceCode.Add(
                sourceGenerator.CreateAssignment(
                    sourceGenerator.CreateSource(sourceProp.Name),
                    sourceGenerator.CreateSource($"d{level}.{destinationProp.Name}")
                )
            );
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