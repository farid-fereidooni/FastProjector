using System;
using FastProjector.Models.TypeMetaDatas;

namespace FastProjector.Models.Assignments
{
    internal abstract class MapBasedPropertyAssignments : PropertyAssignment
    {
        protected ModelMap ModelMap { get; set; }

        public abstract (TypeMetaData sourceType, TypeMetaData destinationType) GetRequiredMapTypes();

        public virtual void AddModelMap(ModelMap modelMap)
        {
            ValidateMap(modelMap);
            ModelMap = modelMap;
        }
        
        private void ValidateMap(ModelMap modelMap)
        {
            var requiredMapTypes = GetRequiredMapTypes();
            
            if (!modelMap.SourceType.Equals(requiredMapTypes.sourceType.TypeInformation))
                throw new ArgumentException("Invalid Metadata passed");
            
            if (!modelMap.DestinationType.Equals(requiredMapTypes.destinationType.TypeInformation))
                throw new ArgumentException("Invalid Metadata passed");
        }
    }
}