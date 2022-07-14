using FastProjector.Models;
using FastProjector.Models.TypeInformations;
using SourceCreationHelper.Interfaces;

namespace FastProjector.Contracts
{
    internal interface IMapRepository
    {
        void Add(ModelMap map);
        ModelMap Get(TypeInformation sourceType, TypeInformation destinationType);
        bool Exists(TypeInformation sourceType, TypeInformation destinationType);
    }
}