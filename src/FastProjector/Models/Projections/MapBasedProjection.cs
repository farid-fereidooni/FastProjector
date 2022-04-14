using System;

namespace FastProjector.Models.Projections
{
    internal abstract class MapBasedProjection : Projection
    {
        private readonly TypeInformation _sourceType;
        private readonly TypeInformation _destinationType;

        public MapBasedProjection(TypeInformation sourceType, TypeInformation destinationType)
        {
            _sourceType = sourceType;
            _destinationType = destinationType;
        }
        protected ModelMap ModelMap { get; set; }

        public void AddModelMap(ModelMap modelMap)
        {
            ModelMap = modelMap;
            ValidateMap();
        }

        private void ValidateMap()
        {
            if(ModelMap is null)
                return;
            
            if (!ModelMap.SourceType.Equals(_sourceType))
                throw new ArgumentException("Invalid Metadata passed");
            
            if (!ModelMap.DestinationType.Equals(_destinationType))
                throw new ArgumentException("Invalid Metadata passed");
        }

    }
}