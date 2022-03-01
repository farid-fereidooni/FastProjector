using FastProjector.MapGenerator.Proccessing;
using FastProjector.MapGenerator.Proccessing.Contracts;
using FastProjector.MapGenerator.Proccessing.Models;
using FastProjector.Test.Helpers;
using SourceCreationHelper.Core;
using Xunit;

namespace FastProjector.Test.PropertyMappingTests;

public class BindTest
{
    private readonly IMapCache _mapCache;
    private readonly ICastingService _castingService;
    public BindTest()
    {
        _castingService = new CastingService();
        _mapCache = new MapCache();
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
        var mapMetadata = new ModelMapMetaData(_mapCache, _castingService, productSymbol, productModelSymbol);
        
        //Assert
        Assert.True(mapMetadata.IsValid);
        Assert.Matches(@"Name\s*=\s*\w\d+\.Name", mapMetadata.ModelMappingSource.Text);
        Assert.Matches(@"Price\s*=\s*\w\d+\.Price", mapMetadata.ModelMappingSource.Text);

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
        var mapMetadata = new ModelMapMetaData(_mapCache, _castingService, productSymbol, productModelSymbol);
        
        //Assert
        Assert.True(mapMetadata.IsValid);
        Assert.DoesNotMatch(@"Price\s*=\s*\w\d+\.Price", mapMetadata.ModelMappingSource.Text);
    }
    
    [Fact]
    public void ModelMapMetaDataCreation_SameTypeMap_IsSuccessful()
    {
        //Arrange
        var sourceBuilder = new SourceBuilder();

        sourceBuilder.AddClass("Category")
            .AddProperty(AccessModifier.@public, "string", "Name");
        
        sourceBuilder.AddClass("CategoryModel")
            .AddProperty(AccessModifier.@public, "string", "Name");
        
        sourceBuilder.AddClass("Product")
            .AddProperty(AccessModifier.@public, "int", "Price")
            .AddProperty(AccessModifier.@public, "Category", "Category");
            
        sourceBuilder.AddClass("ProductModel")
            .AddProperty(AccessModifier.@public, "DateTime", "Price")
            .AddProperty(AccessModifier.@public, "CategoryModel", "Category");
        
        var compilation = sourceBuilder.GetCompilation();
        
        var productSymbol = compilation.GetClassSymbol("Product");
        var productModelSymbol = compilation.GetClassSymbol("ProductModel");
        
        //Act
        var mapMetadata = new ModelMapMetaData(_mapCache, _castingService, productSymbol, productModelSymbol);
        
        //Assert
        Assert.True(mapMetadata.IsValid);
        Assert.DoesNotMatch(@"Price\s*=\s*\w\d+\.Price", mapMetadata.ModelMappingSource.Text);
    }
}

