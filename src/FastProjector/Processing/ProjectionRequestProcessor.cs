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
        private readonly IMapRepository _mapRepository;
        private readonly IMapResolverService _mapResolverService;

        public ProjectionRequestProcessor(IModelMapService mapService, IMapRepository mapRepository, IMapResolverService mapResolverService)
        {
            _mapService = mapService;
            _mapRepository = mapRepository;
            _mapResolverService = mapResolverService;
        }
        public string ProcessProjectionRequest(IEnumerable<ProjectionRequest> requests)
        {
            var mappings = new List<ModelMap>();
            
            foreach(var item in requests)
            {
                var modelMetadata = new ModelMapMetaData(item.ProjectionSource, item.ProjectionTarget);
                
                if(_mapRepository.Exists(modelMetadata.SourceTypeInformation, modelMetadata.DestinationTypeInformation))
                    continue;
                
                var mapping = new ModelMap(modelMetadata);

                _mapRepository.Add(mapping);
            }

            return CreateAllMappingSource(mappings);
        }
        
        public string CreateAllMappingSource(List<ModelMap> mappings)
        {
            throw new NotImplementedException();
        }
    }    
}