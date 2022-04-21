using System;
using FastProjector.Models.TypeMetaDatas;

namespace FastProjector.Models.Assignments
{
    internal abstract class MapBasedPropertyAssignments : PropertyAssignment
    {
        private readonly ClassTypeMetaData _sourceType;

 
        protected ModelMap ModelMap { get; set; }

        public void AddModelMap(ModelMap modelMap)
        {
            ModelMap = modelMap;
            ValidateMap();
        }

        protected abstract void ValidateMap();
    }
}