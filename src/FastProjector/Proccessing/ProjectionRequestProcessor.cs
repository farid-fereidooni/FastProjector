using System;
using System.Collections.Generic;
using FastProjector.MapGenerator.Analyzing;
using FastProjector.MapGenerator.Proccessing.Contracts;
using FastProjector.MapGenerator.Proccessing.Models;

namespace FastProjector.MapGenerator.Proccessing
{
    
    internal class ProjectionRequestProcessor : IProjectionRequestProcessor
    {
        private readonly ICastingService _castingService;
        private readonly IMapCache _mapCache;

        public ProjectionRequestProcessor(IMapCache mapCache, ICastingService castingService)
        {
            _mapCache = mapCache;
            _castingService = castingService;
        }
        public string ProcessProjectionRequest(IEnumerable<ProjectionRequest> requests)
        {
            var mappings = new List<ModelMapMetaData>();
            
            foreach(var item in requests)
            {
                
                var mapping = new ModelMapMetaData(_mapCache, _castingService, item.ProjectionSource,
                    item.ProjectionTarget);
                
                if(!mapping.IsValid)
                    continue;
                
                mappings.Add(mapping);
            }

            return CreateAllMappingSource(mappings);
        }

        public string CreateAllMappingSource(List<ModelMapMetaData> mappings)
        {
            throw new NotImplementedException();
        }
    }    
}