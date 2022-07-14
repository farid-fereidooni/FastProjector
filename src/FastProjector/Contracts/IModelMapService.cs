using FastProjector.Models;
using FastProjector.Models.Casting;
using FastProjector.Models.TypeInformations;
using Microsoft.CodeAnalysis;
using SourceCreationHelper.Interfaces;

namespace FastProjector.Contracts
{
    internal interface IModelMapService
    {
        PropertyCastResult CastType(TypeInformation sourceType, TypeInformation destinationType);
        string GetNewProjectionVariableName();
    }
}