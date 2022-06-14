using System;
using FastProjector.Models.TypeInformations;
using SourceCreationHelper;
using SourceCreationHelper.Interfaces;

namespace FastProjector.Models.Projections
{
    internal abstract class MapBasedProjection : Projection, IMapBasedProjection
    {
        private readonly CollectionTypeInformation _destinationTypeInformation;
        private readonly CollectionTypeInformation _sourceTypeInformation;

        protected MapBasedProjection(CollectionTypeInformation destinationTypeInformation, CollectionTypeInformation sourceTypeInformation) : base(destinationTypeInformation)
        {
            _destinationTypeInformation = destinationTypeInformation;
            _sourceTypeInformation = sourceTypeInformation;
        }
        
        protected ModelMap ModelMap { get; private set; }

        public virtual void AddModelMap(ModelMap modelMap)
        {
            ModelMap = modelMap;
            ValidateMap();
        }

        private void ValidateMap()
        {
            if(ModelMap is null)
                return;
            
            if (!ModelMap.SourceType.Equals(_sourceTypeInformation))
                throw new ArgumentException("Invalid Metadata passed");
            
            if (!ModelMap.DestinationType.Equals(_destinationTypeInformation))
                throw new ArgumentException("Invalid Metadata passed");
        }

    }
}