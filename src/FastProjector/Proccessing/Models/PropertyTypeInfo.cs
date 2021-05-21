using FastProjector.MapGenerator.Proccessing.Models;
using Microsoft.CodeAnalysis;

namespace FastProjector.MapGenerator.Proccessing.Models
{
    internal class PropertyTypeInfo
    {
        public PropertyTypeEnum Type { get;set; }
        public string FullName { get;set;}
        public bool IsGenericClass { get;set; }
        public bool IsNonGenericClass { get;set; }
        public bool IsGenericEnumerable { get; set; }
        public bool IsPrimitive { get; set;}
 

    }
}