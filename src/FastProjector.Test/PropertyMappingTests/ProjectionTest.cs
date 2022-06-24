using FastProjector.Models;
using FastProjector.Models.Casting;
using FastProjector.Services;
using FastProjector.Test.Helpers;
using SourceCreationHelper;
using SourceCreationHelper.Core;
using Xunit;

namespace FastProjector.Test.PropertyMappingTests;

public class ProjectionTest
{
    
    private readonly ModelMapService _mapService;

    public ProjectionTest()
    {
        _mapService = new ModelMapService(new MapCache(), new CastingService(DefaultCastingConfigurations.GetConfigurations()), new VariableNameGenerator());
    }
    
    [Fact]
    public void ModelMapMetaDataCreation_simpleSameTypeBind_IsSuccessful()
    {
        //Arrange
        var sourceBuilder = new SourceBuilder();
        sourceBuilder.AddClass("Product")
            .AddProperty(AccessModifier.@public, "string", "Name")
            .AddProperty(AccessModifier.@public, "int", "Price");

        sourceBuilder.AddClass("ProductModel")
            .AddProperty(AccessModifier.@public, "string", "Name")
            .AddProperty(AccessModifier.@public, "int", "Price");
        
        var compilation = sourceBuilder.GetCompilation();
        
        var productSymbol = compilation.GetClassSymbol("Product");
        var productModelSymbol = compilation.GetClassSymbol("ProductModel");
        
        //Act
        var modelMap = new ModelMapMetaData( productSymbol, productModelSymbol)
            .CreateModelMap(_mapService);
        var sourceText = modelMap.CreateMappingSource(_mapService, SourceCreator.CreateSource("a")).Text;
        
        //Assert
        Assert.Matches(@"Name\s*=\s*a\.Name", sourceText);
        Assert.Matches(@"Price\s*=\s*a\.Price", sourceText);

    }

}