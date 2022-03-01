using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace FastProjector.Test.Helpers;

public static class CompilationExtensions
{
    public static ITypeSymbol? GetClassSymbol(this CSharpCompilation compilation, string name)
    {
        return compilation.GetSymbolsWithName(name).FirstOrDefault() as ITypeSymbol;
    }
}