using System;
using FastProjector.Models.TypeMetaDatas;

namespace FastProjector.Models.Assignments
{
    internal abstract class MapBasedPropertyAssignments : PropertyAssignment
    {
        protected ModelMap ModelMap { get; set; }

        public virtual void AddModelMap(ModelMap modelMap)
        {
            ModelMap = modelMap;
        }
    }
}