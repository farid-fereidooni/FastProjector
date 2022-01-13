using FastProjector.MapGenerator.Proccessing.Models;
using Microsoft.CodeAnalysis;

namespace FastProjector.MapGenerator.Proccessing.Contracts
{
    internal interface IPropertyCasting
    {
        PropertyCastResult CastType(IPropertySymbol sourceProp, IPropertySymbol destinationProp);
        PropertyCastResult CastType(PropertyTypeInformation sourceProp, IPropertySymbol destinationProp);
        PropertyCastResult CastType(IPropertySymbol sourceProp, PropertyTypeInformation destinationProp);
        PropertyCastResult CastType(PropertyTypeInformation sourceProp, PropertyTypeInformation destinationProp);
    }
}