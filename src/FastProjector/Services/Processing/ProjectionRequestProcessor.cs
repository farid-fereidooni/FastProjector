using System;
using System.Collections.Generic;
using FastProjector.Analyzing;
using FastProjector.Contracts;
using FastProjector.Models;

namespace FastProjector.Services.Processing
{
    
    internal class ProjectionRequestProcessor : IProjectionRequestProcessor
    {
        private readonly IMapRepository _mapRepository;
        private readonly IMapResolverService _mapResolverService;

        public ProjectionRequestProcessor(IMapRepository mapRepository, IMapResolverService mapResolverService)
        {
            _mapRepository = mapRepository;
            _mapResolverService = mapResolverService;
        }
        public void ProcessProjectionRequest(IEnumerable<ProjectionRequest> requests)
        {
            var notResolvedMappings = new List<ModelMap>();
            
            foreach(var item in requests)
            {
                var modelMetadata = new ModelMapMetaData(item.ProjectionSource, item.ProjectionTarget);
                
                if(_mapRepository.Exists(modelMetadata.SourceTypeInformation, modelMetadata.DestinationTypeInformation))
                    continue;
                
                var mapping = new ModelMap(modelMetadata);
                
                mapping.TryResolveRequiredMaps(_mapResolverService);

                if (mapping.RequiresModelMaps())
                    notResolvedMappings.Add(mapping);

                _mapRepository.Add(mapping);
            }

            TryResolveRequiredMapsAfterAllMappingProcessed(notResolvedMappings);
        }

        private void TryResolveRequiredMapsAfterAllMappingProcessed(List<ModelMap> notResolvedMappings)
        {
            foreach (var mapping in notResolvedMappings)
            {
                mapping.TryResolveRequiredMaps(_mapResolverService);
            }
        }
    }
}