using FastProjector.Models;
using FastProjector.Models.PropertyTypeInformations;
using Microsoft.CodeAnalysis;
using SourceCreationHelper.Interfaces;

namespace FastProjector.Contracts
{
    internal interface IModelMapService
    {
        ISourceText FetchFromCache(PropertyTypeInformation sourceType, PropertyTypeInformation destinationType);
        ModelMapMetaData CreateSameTypeMap(ITypeSymbol type);
        PropertyCastResult CastType(PropertyTypeInformation sourceType, PropertyTypeInformation destinationType);
        string GetNewProjectionVariableName();
    }
}