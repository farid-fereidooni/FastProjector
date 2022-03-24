using FastProjector.Models;
using FastProjector.Models.PropertyTypeInformations;

namespace FastProjector.Contracts
{
    internal interface ICastingService
    {
        PropertyCastResult CastType(PropertyTypeInformation sourceProp, PropertyTypeInformation destinationProp);
    }
}