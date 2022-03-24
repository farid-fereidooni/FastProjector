using Microsoft.CodeAnalysis;

namespace FastProjector.MapGenerator.Proccessing.Models
{
    internal interface IPropertyTypeInformationFactory
    {
        PropertyTypeInformation CreatePropertyTypeInformation(ITypeSymbol typeSymbol);
        PropertyTypeInformation CreatePropertyTypeInformation(IPropertySymbol propertySymbol);
    }
}