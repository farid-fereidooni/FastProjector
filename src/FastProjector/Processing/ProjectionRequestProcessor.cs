using System;
using System.Collections.Generic;
using FastProjector.Analyzing;
using FastProjector.Contracts;
using FastProjector.Models;
using FastProjector.Models.Assignments;

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
            var mappings = new List<ModelMap>();
            
            foreach(var item in requests)
            {
                //TODO: check if already done
                var modelMetadata = new ModelMapMetaData(item.ProjectionSource, item.ProjectionTarget);

                var mapping = modelMetadata.CreateModelMap(_mapService);
   
                mappings.Add(mapping);
            }

            return CreateAllMappingSource(mappings);
        }

        private IEnumerable<PropertyAssignment> CreateOrFetchFromCacheAssignments(IEnumerable<PropertyMapMetaData> assignableProperties)
        {
            throw new NotImplementedException();
        }

        public string CreateAllMappingSource(List<ModelMap> mappings)
        {
            throw new NotImplementedException();
        }
    }    
}