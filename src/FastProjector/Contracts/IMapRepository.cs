using System.Collections.Generic;
using FastProjector.Models;
using FastProjector.Models.TypeInformations;
using SourceCreationHelper.Interfaces;

namespace FastProjector.Contracts
{
    internal interface IMapRepository
    {
        void Add(ModelMap map);
        ModelMap Get(TypeInformation sourceType, TypeInformation destinationType);
        IReadOnlyDictionary<string, IReadOnlyCollection<ModelMap>> GetAll(); 
        bool Exists(TypeInformation sourceType, TypeInformation destinationType);
    }
}