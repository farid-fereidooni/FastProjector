using System;
using FastProjector.Models.PropertyMetaDatas;

namespace FastProjector.Models.Assignments
{
    internal abstract class MapBasedPropertyAssignments : PropertyAssignment
    {
        private readonly ClassPropertyMetaData _sourceProperty;

 
        protected ModelMap ModelMap { get; set; }

        public void AddModelMap(ModelMap modelMap)
        {
            ModelMap = modelMap;
            ValidateMap();
        }

        protected abstract void ValidateMap();
    }
}