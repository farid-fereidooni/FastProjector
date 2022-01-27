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
        private readonly IMapCache _mapService;

        public ProjectionRequestProcessor(IMapCache mapService, ICastingService castingService)
        {
            _castingService = new CastingService();
            _mapService = mapService;
            _castingService = castingService;
        }
        public string ProcessProjectionRequest(IEnumerable<ProjectionRequest> requests)
        {
            var mappings = new List<ModelMapMetaData>();
            
            foreach(var item in requests)
            {
                var mapping = new ModelMapMetaData(_mapService, _castingService, item.ProjectionSource,
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