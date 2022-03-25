using FastProjector.Models;
using FastProjector.Services;
using FastProjector.Test.Helpers;
using SourceCreationHelper.Core;
using Xunit;
using static FastProjector.Test.Helpers.RegexHelper;

namespace FastProjector.Test.PropertyMappingTests;

public class BindTest
{
    private readonly ModelMapService _mapService;

    public BindTest()
    {
        _mapService = new ModelMapService(new MapCache(), new CastingService(), new VariableNameGenerator());
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
        var mapMetadata = new ModelMapMetaData( productSymbol, productModelSymbol);
        
        //Assert
        Assert.Matches(@"Name\s*=\s*\w\d+\.Name", mapMetadata.CreateMappingSource(_mapService).Text);
        Assert.Matches(@"Price\s*=\s*\w\d+\.Price", mapMetadata.CreateMappingSource(_mapService).Text);

    }
    
    [Fact]
    public void ModelMapMetaDataCreation_simpleNotSameTypeBind_IsFailed()
    {
        //Arrange
        var sourceBuilder = new SourceBuilder();
        sourceBuilder.AddClass("Product")
            .AddProperty(AccessModifier.@public, "int", "Price");

        sourceBuilder.AddClass("ProductModel")
            .AddProperty(AccessModifier.@public, "DateTime", "Price");
        
        var compilation = sourceBuilder.GetCompilation();
        
        var productSymbol = compilation.GetClassSymbol("Product");
        var productModelSymbol = compilation.GetClassSymbol("ProductModel");
        
        //Act
        var mapMetadata = new ModelMapMetaData( productSymbol, productModelSymbol);
        
        //Assert
        Assert.DoesNotMatch(@"Price\s*=\s*\w\d+\.Price", mapMetadata.CreateMappingSource(_mapService).Text);
    }
    
    [Fact]
    public void ModelMapMetaDataCreation_SameTypeMap_IsSuccessful()
    {
        //Arrange
        var sourceBuilder = new SourceBuilder();

        sourceBuilder.AddClass("Category")
            .AddProperty(AccessModifier.@public, "string", "Name");

        sourceBuilder.AddClass("Product")
            .AddProperty(AccessModifier.@public, "int", "Price")
            .AddProperty(AccessModifier.@public, "Category", "Category");
            
        sourceBuilder.AddClass("ProductModel")
            .AddProperty(AccessModifier.@public, "DateTime", "Price")
            .AddProperty(AccessModifier.@public, "Category", "Category");
        
        var compilation = sourceBuilder.GetCompilation();
        
        var productSymbol = compilation.GetClassSymbol("Product");
        var productModelSymbol = compilation.GetClassSymbol("ProductModel");
        
        //Act
        var mapMetadata = new ModelMapMetaData( productSymbol, productModelSymbol);
        
        //Assert
        Assert.Matches(
            $@"Category = new {AnyNamespace}Category 
                                            {{ 
                                                {Anything}
                                                Name = {AnyNamespace}Name
                            ".ReplaceSpaceWithAnySpace()
            , mapMetadata.CreateMappingSource(_mapService).Text);
    }
}

