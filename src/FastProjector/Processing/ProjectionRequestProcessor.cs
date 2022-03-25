using System;
using System.Collections.Generic;
using FastProjector.Analyzing;
using FastProjector.Contracts;
using FastProjector.Models;

namespace FastProjector.Processing
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
                //TODO: check if already done

                var mapping = new ModelMapMetaData(item.ProjectionSource,
                    item.ProjectionTarget);
                
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