using System;
using System.Collections.Generic;
using FastProjector.MapGenerator.Analyzing;
using FastProjector.MapGenerator.Proccessing.Contracts;
using FastProjector.MapGenerator.Proccessing.Models;

namespace FastProjector.MapGenerator.Proccessing
{
    
    internal class RequestProcessing
    {
        private readonly IPropertyCasting _propertyCasting;
        private readonly IMapCache _mapCache;

        public RequestProcessing(IMapCache mapCache, IPropertyCasting propertyCasting)
        {
            _propertyCasting = new PropertyCasting();
            _mapCache = mapCache;
            _propertyCasting = propertyCasting;
        }
        public string ProcessProjectionRequest(IEnumerable<ProjectionRequest> requests)
        {
            var mappings = new List<ModelMapMetaData>();
            foreach(var item in requests)
            {
                var mapping = new ModelMapMetaData(_mapCache,_propertyCasting, item.ProjectionSource, item.ProjectionTarget);
                
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