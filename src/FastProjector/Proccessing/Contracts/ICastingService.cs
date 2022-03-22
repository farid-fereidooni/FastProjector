using FastProjector.MapGenerator.Proccessing.Models;
using Microsoft.CodeAnalysis;

namespace FastProjector.MapGenerator.Proccessing.Contracts
{
    internal interface ICastingService
    {
        PropertyCastResult CastType(PropertyTypeInformation sourceProp, PropertyTypeInformation destinationProp);
    }
}