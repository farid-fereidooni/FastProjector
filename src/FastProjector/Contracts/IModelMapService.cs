using FastProjector.Models;
using FastProjector.Models.PropertyTypeInformations;
using Microsoft.CodeAnalysis;
using SourceCreationHelper.Interfaces;

namespace FastProjector.Contracts
{
    internal interface IModelMapService
    {
        ModelMap FetchFromCache(PropertyTypeInformation sourceType, PropertyTypeInformation destinationType);
        void AddToCache(ModelMap modelMap);
        PropertyCastResult CastType(PropertyTypeInformation sourceType, PropertyTypeInformation destinationType);
        string GetNewProjectionVariableName();
    }
}