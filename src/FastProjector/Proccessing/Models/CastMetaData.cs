using Microsoft.CodeAnalysis;

namespace FastProjector.MapGenerator.Proccessing.Models
{
    public class CastMetaData
    {
        public SpecialType Type { get;set; }
        public bool HasGenericType {get;set;}
        public GenericTypeMetaData[] GenericTypes {get;set;}
    }
    public class GenericTypeMetaData {
        public string FullName {get;set;}
        public bool IsCustomClass {get;set;}
    }
}