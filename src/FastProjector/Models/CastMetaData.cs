namespace FastProjector.Models
{
    internal class CastMetaData
    {
        public string FullName { get; set; }
        public PropertyType Type { get; set; }
        public bool HasGenericType { get; set; }
        public PropertyTypeCategoryEnum TypeCategory { get; set; }
        public GenericMetaData[] GenericTypes {get;set;}
    }

    internal class GenericMetaData
    {
        public string FullName { get; set; }
        public bool IsPrimitive { get; set; }
    }
}