using FastProjector.MapGenerator.Proccessing;
using FastProjector.MapGenerator.Proccessing.Contracts;
using FastProjector.MapGenerator.Proccessing.Models;
using FastProjector.Test.Helpers;
using SourceCreationHelper.Core;
using Xunit;
using static FastProjector.Test.Helpers.RegexHelper;

namespace FastProjector.Test.PropertyMappingTests;

public class CastTest
{
  private readonly IMapCache _mapCache;
    private readonly ICastingService _castingService;
    public CastTest()
    {
        _castingService = new CastingService();
        _mapCache = new MapCache();
    }
    
    
    [Theory]
    [InlineData("int")]
    [InlineData("bool")]
    [InlineData("Single")]
    [InlineData("double")]
    [InlineData("decimal")]
    public void ModelMapMetaDataCreation_CastTypesToString_IsSuccessful(string sourceType)
    {
        //Arranges
        var sourceBuilder = new SourceBuilder();
        sourceBuilder.AddClass("Product")
            .AddProperty(AccessModifier.@public, sourceType, "Price");

        sourceBuilder.AddClass("ProductModel")
            .AddProperty(AccessModifier.@public, "string", "Price");
        
        var compilation = sourceBuilder.GetCompilation();
        
        var productSymbol = compilation.GetClassSymbol("Product");
        var productModelSymbol = compilation.GetClassSymbol("ProductModel");
        
        //Act
        var mapMetadata = new ModelMapMetaData(_mapCache, _castingService, productSymbol, productModelSymbol);
        
        //Assert
        Assert.True(mapMetadata.IsValid);
        Assert.Matches($@"Price = {AnyNamespace}Price.ToString()".ReplaceSpaceWithAnySpace(), mapMetadata.ModelMappingSource.Text);
    }
    
    
    [Theory]
    [InlineData("byte")]
    [InlineData("short")]
    public void ModelMapMetaDataCreation_CastTypesToInt_IsSuccessful(string sourceType)
    {
        //Arranges
        var sourceBuilder = new SourceBuilder();
        sourceBuilder.AddClass("Product")
            .AddProperty(AccessModifier.@public, sourceType, "Price");

        sourceBuilder.AddClass("ProductModel")
            .AddProperty(AccessModifier.@public, "int", "Price");
        
        var compilation = sourceBuilder.GetCompilation();
        
        var productSymbol = compilation.GetClassSymbol("Product");
        var productModelSymbol = compilation.GetClassSymbol("ProductModel");
        
        //Act
        var mapMetadata = new ModelMapMetaData(_mapCache, _castingService, productSymbol, productModelSymbol);
        
        //Assert
        Assert.True(mapMetadata.IsValid);
        Assert.Matches($@"Price = (int){AnyNamespace}Price".ReplaceSpaceWithAnySpace(), mapMetadata.ModelMappingSource.Text);
    }
    
}