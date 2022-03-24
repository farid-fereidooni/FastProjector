using System;
using System.Collections.Generic;
using FastProjector.MapGenerator.Analyzing;
using FastProjector.MapGenerator.Proccessing.Contracts;
using FastProjector.MapGenerator.Proccessing.Models;
using FastProjector.MapGenerator.Proccessing.Services;

namespace FastProjector.MapGenerator.Proccessing
{
    
    internal class ProjectionRequestProcessor : IProjectionRequestProcessor
    {
        private readonly IModelMapService _mapService;

        public ProjectionRequestProcessor(IModelMapService mapService)
        {
            _mapService = mapService;
        }
        public string ProcessProjectionRequest(IEnumerable<ProjectionRequest> requests)
        {
            var mappings = new List<ModelMapMetaData>();
            
            foreach(var item in requests)
            {
                
                var mapping = _mapService.CreateOrFetchFromCache(item.ProjectionSource,
                    item.ProjectionTarget, 1);
                
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