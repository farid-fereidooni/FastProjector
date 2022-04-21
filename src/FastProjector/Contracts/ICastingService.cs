using FastProjector.Models;
using FastProjector.Models.TypeInformations;

namespace FastProjector.Contracts
{
    internal interface ICastingService
    {
        PropertyCastResult CastType(TypeInformation sourceProp, TypeInformation destinationProp);
    }
}