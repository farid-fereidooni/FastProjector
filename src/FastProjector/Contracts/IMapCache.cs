using FastProjector.Models;
using SourceCreationHelper.Interfaces;

namespace FastProjector.Contracts
{
    internal interface IMapCache
    {
        void Add(TypeInformation sourceType, TypeInformation destinationType, ISourceText sourceText);
        ISourceText Get(TypeInformation sourceType, TypeInformation destinationType);
        bool Exists(TypeInformation sourceType, TypeInformation destinationType);
    }
}