using Microsoft.CodeAnalysis;

namespace FastProjector.MapGenerator.Proccessing
{
    public static class SemanticApiHelper
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
    }
}