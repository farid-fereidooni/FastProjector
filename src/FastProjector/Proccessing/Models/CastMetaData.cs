using Microsoft.CodeAnalysis;

namespace FastProjector.MapGenerator.Proccessing.Models
{
    public class CastMetaData
    {
        public string FullName {get;set;}
        public PropertyTypeCategoryEnum TypeCategory {get;set;}
        public SpecialType Type { get;set; }
        public bool HasGenericType {get;set;}
        public CastMetaData[] GenericTypes {get;set;}
    }
}