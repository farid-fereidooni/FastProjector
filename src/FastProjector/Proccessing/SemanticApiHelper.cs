using System;
using System.Collections.Generic;
using System.Linq;
using FastProjector.MapGenerator.Proccessing.Models;
using Microsoft.CodeAnalysis;

namespace FastProjector.MapGenerator.Proccessing
{
    internal static class SemanticApiHelper
    {
        public static string GetFullNamespace(this ISymbol symbol)
        {
            if ((symbol.ContainingNamespace == null) ||
                string.IsNullOrEmpty(symbol.ContainingNamespace.Name))
            {
                return null;
            }

            // get the rest of the full namespace string
            string restOfResult = symbol.ContainingNamespace.GetFullNamespace();

            string result = symbol.ContainingNamespace.Name;

            if (restOfResult != null)
                // if restOfResult is not null, append it after a period
                result = restOfResult + '.' + result;

            return result;
        }

        public static string GetFullName(this ISymbol symbol)
        {
            var fullNameSpace = symbol.GetFullNamespace();
            if (fullNameSpace != null)
            {
                return $"{fullNameSpace}.{symbol.Name}";
            }

            return null;
        }

        public static bool IsGeneric(this ITypeSymbol typeSymbol)
        {
            return (typeSymbol is INamedTypeSymbol namedTypeSymbol && namedTypeSymbol.Arity > 0 &&
                    namedTypeSymbol.TypeArguments.NotNullAny());
        }

        public static TypeInformation ToTypeInformation(this ITypeSymbol typeSymbol)
        {
            return new TypeInformation(typeSymbol);
        }


        public static IEnumerable<IPropertySymbol> ExtractProps(this ITypeSymbol classSymbol)
        {
            if (!classSymbol.IsClass())
            {
                throw new Exception("symbol is not class");
            }

            return classSymbol.GetMembers().OfType<IPropertySymbol>();
        }

        public static bool IsPublic(this IPropertySymbol property)
        {
            return property.DeclaredAccessibility == Accessibility.Public;
        }

        public static bool IsSettable(this IPropertySymbol property)
        {
            return IsPublic(property) && !property.IsReadOnly && property.SetMethod != null;
        }

        public static bool IsClass(this ITypeSymbol symbol)
        {
            return symbol.TypeKind == TypeKind.Class;
        }
        
        public static bool HasParameterlessConstructor(this ITypeSymbol symbol)
        {
            if (!symbol.IsClass())
                throw new Exception("symbol is not class");

            var members = symbol.GetMembers();
            return false;
        }
        

        private static SyntaxNode GetNodeOfSymbol(ISymbol symbol)
        {

            var location = symbol.Locations.FirstOrDefault();
            return location != null ? location.SourceTree?.GetRoot()?.FindNode(location.SourceSpan) : null;
        }
        
        

    }
}