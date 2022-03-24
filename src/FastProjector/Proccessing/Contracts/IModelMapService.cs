using FastProjector.MapGenerator.Proccessing.Models;
using Microsoft.CodeAnalysis;

namespace FastProjector.MapGenerator.Proccessing.Services
{
    internal interface IModelMapService
    {
        ModelMapMetaData CreateOrFetchFromCache(ITypeSymbol sourceType, ITypeSymbol destinationType, int level);
    }
}