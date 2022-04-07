using FastProjector.Models;
using SourceCreationHelper.Interfaces;

namespace FastProjector.Contracts
{
    internal interface IMapCache
    {
        void Add(ModelMap map);
        ModelMap Get(TypeInformation sourceType, TypeInformation destinationType);
        bool Exists(TypeInformation sourceType, TypeInformation destinationType);
    }
}