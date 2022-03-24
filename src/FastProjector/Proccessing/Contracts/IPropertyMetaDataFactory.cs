using Microsoft.CodeAnalysis;

namespace FastProjector.MapGenerator.Proccessing.Models
{
    internal interface IPropertyMetaDataFactory
    {
        PropertyMetaData CreatePropertyMetaData(IPropertySymbol propertySymbol);
    }
}