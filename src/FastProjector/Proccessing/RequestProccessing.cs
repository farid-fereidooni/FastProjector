using System;
using System.Collections.Generic;
using System.Linq;
using FastProjector.MapGenerator.Analyzing;
using FastProjector.MapGenerator.DevTools;
using FastProjector.MapGenerator.Proccessing.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FastProjector.MapGenerator.Proccessing
{
    internal static class RequestProccessing
    {
        
        public static string ProccessProjectionRequest(IEnumerable<ProjectionRequest> requests)
        { 
            foreach(var item in requests)
            {
                CreateMappings(item);
            }
            return "";
        }

        public static void CreateMappings(ProjectionRequest request)
        {
              
            var sourceProps = ExtractProps(request.ProjectionSource, PropertyAccessTypeEnum.HasPublicGet);
            var destinationProps = new LinkedList<IPropertySymbol>(ExtractProps(request.ProjectionTarget, PropertyAccessTypeEnum`.HasPublicSet));

            foreach(var sourceProp in sourceProps)
            {
                

                Logger.Log(sourceProp.Type.Name);
                Logger.Log(sourceProp.Type.GetFullNamespace());
                Logger.Log(sourceProp.Type.TypeKind.ToString());
                if(sourceProp.Type is INamedTypeSymbol namedType)
                {
                    Logger.Log(namedType.MetadataName);
                }
                if(sourceProp.Type is IArrayTypeSymbol arrayType)
                {
                    Logger.Log(arrayType.MetadataName);
                }
                Logger.Log("=========");
                // var destinationNode = destinationProps.First;
                // while(destinationNode != null)
                // {
                //     if(destinationNode.Value.Name == sourceProp.Name)
                //     {
                        
                //     }
                //     destinationNode = destinationNode.Next;
                // }               
            }
            
        }

        public static IEnumerable<IPropertySymbol> ExtractProps(INamedTypeSymbol classSymbol, PropertyAccessTypeEnum? propertyTypeToSearch = null )
        {
            List<IPropertySymbol> result = new List<IPropertySymbol>();
            var mems = classSymbol.GetMembers();
            foreach(var mem in mems)
            {
                
                if(mem is IPropertySymbol propSymbol)
                {              
                    bool isFiltered = false;    
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
            }
            return result;
        }

        public static SyntaxNode GetNodeOfSymbol(ISymbol symbol)
        {
            
            var location = symbol.Locations.FirstOrDefault();
            if(location != null)
            {
                return location.SourceTree?.GetRoot()?.FindNode(location.SourceSpan);
            }

            return null;
        }

        private static bool IsPropertyPublic(IPropertySymbol property)
        {
            var node = GetNodeOfSymbol(property);

            if(node != null)
            {
                if (node is PropertyDeclarationSyntax propertyNode)
                {
                    return propertyNode.Modifiers.Any(a => a.ValueText == "public");
                }
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

        private static PropertyMapMetaData CreateBinding(IPropertySymbol sourceProp, IPropertySymbol destinationProp)
        {
            Logger.Log(sourceProp.ContainingType.Name);
            return null;
        }
       
        
    }    
}