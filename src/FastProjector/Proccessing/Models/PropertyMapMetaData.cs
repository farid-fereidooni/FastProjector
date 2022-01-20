namespace FastProjector.MapGenerator.Proccessing.Models
{
    internal class PropertyMapMetaData
    {
        public PropertyMapMetaData(PropertyMetaData sourceProperty, PropertyMetaData destinationProperty)
        {
            SourceProperty = sourceProperty;
            DestinationProperty = destinationProperty;
        }
        public PropertyMetaData SourceProperty { get; }
        public PropertyMetaData DestinationProperty { get; }
    }
}