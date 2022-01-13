using FastProjector.MapGenerator.Proccessing.Models;

namespace FastProjector.MapGenerator.Proccessing.Contracts
{
    internal interface IMapCache
    {
        void Add(ModelMapMetaData mapMetaData);
        ModelMapMetaData Get(TypeInformation sourceType, TypeInformation destinationType);
        bool Exists(TypeInformation sourceType, TypeInformation destinationType);
    }
}