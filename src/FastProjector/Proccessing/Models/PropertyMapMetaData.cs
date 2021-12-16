using Microsoft.CodeAnalysis;

namespace FastProjector.MapGenerator.Proccessing.Models
{
    internal class PropertyMapMetaData
    {
        public IPropertySymbol SourceProperty { get; set; }
        public IPropertySymbol DestinationProperty { get; set; }
    }
}