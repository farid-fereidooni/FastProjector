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
           if(fullNameSpace != null)
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
        
        
        
    }
}