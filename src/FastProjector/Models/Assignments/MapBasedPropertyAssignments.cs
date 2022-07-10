using System;
using FastProjector.Models.TypeMetaDatas;

namespace FastProjector.Models.Assignments
{
    internal abstract class MapBasedPropertyAssignments : PropertyAssignment, IRequireMapData
    {
        protected ModelMap ModelMap { get; set; }

        public abstract (ClassTypeMetaData sourceType, ClassTypeMetaData destinationType) GetRequiredMapTypes();

        public virtual void AddModelMap(ModelMap modelMap)
        {
            ValidateMap(modelMap);
            ModelMap = modelMap;
        }
        
        public virtual bool HasModelMap()
        {
            return ModelMap is not null;
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