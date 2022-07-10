using FastProjector.Models;
using FastProjector.Models.TypeMetaDatas;

namespace FastProjector.Contracts
{
    internal interface IMapResolverService
    {
        ModelMap ResolveMap(ClassTypeMetaData sourceType, ClassTypeMetaData destinationType);
    }
}