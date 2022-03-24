using FastProjector.MapGenerator.Proccessing.Contracts;
using FastProjector.MapGenerator.Proccessing.Models;
using FastProjector.MapGenerator.Proccessing.Models.Assignments;
using Microsoft.CodeAnalysis;

namespace FastProjector.MapGenerator.Proccessing.Services
{
    internal class ModelMapService : IModelMapService
    {
        private readonly IMapCache _mapCache;
        private readonly IPropertyMetaDataFactory _metaDataFactory;
        private readonly IPropertyAssignmentFactory _assignmentFactory;

        public ModelMapService(IMapCache mapCache, IPropertyMetaDataFactory metaDataFactory, IPropertyAssignmentFactory assignmentFactory)
        {
            _mapCache = mapCache;
            _metaDataFactory = metaDataFactory;
            _assignmentFactory = assignmentFactory;
        }
        public ModelMapMetaData CreateOrFetchFromCache(ITypeSymbol sourceType, ITypeSymbol destinationType, int level)
        {
             return  _mapCache.Get(sourceType.ToTypeInformation(), destinationType.ToTypeInformation()) ??
                     new ModelMapMetaData(sourceType, destinationType, _metaDataFactory, _assignmentFactory, level);
        }
    }
}