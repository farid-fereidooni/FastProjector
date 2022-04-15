using FastProjector.Models;
using FastProjector.Models.TypeInformations;
using Microsoft.CodeAnalysis;
using SourceCreationHelper.Interfaces;

namespace FastProjector.Contracts
{
    internal interface IModelMapService
    {
        ModelMap FetchFromCache(TypeInformation sourceType, TypeInformation destinationType);
        void AddToCache(ModelMap modelMap);
        PropertyCastResult CastType(TypeInformation sourceType, TypeInformation destinationType);
        string GetNewProjectionVariableName();
    }
}