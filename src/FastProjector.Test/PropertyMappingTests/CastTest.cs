using FastProjector.Contracts;
using FastProjector.Models;
using FastProjector.Models.Casting;
using FastProjector.Services;
using FastProjector.Test.Helpers;
using SourceCreationHelper;
using SourceCreationHelper.Core;
using Xunit;
using static FastProjector.Test.Helpers.RegexHelper;

namespace FastProjector.Test.PropertyMappingTests;

public class CastTest
{
    private readonly IModelMapService _mapService;

    public CastTest()
    {
        _mapService = new ModelMapService(new MapCache(), new CastingService(DefaultCastingConfigurations.GetConfigurations()), new VariableNameGenerator());
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
        var modelMap = new ModelMapMetaData(productSymbol, productModelSymbol)
            .CreateModelMap(_mapService);
        
        //Assert
        Assert.Matches($@"Price = {AnyNamespace}Price.ToString()".ReplaceSpaceWithAnySpace(), 
            modelMap.CreateMappingSource(_mapService, SourceCreator.CreateSource("a")).Text);
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
        var modelMap = new ModelMapMetaData(productSymbol, productModelSymbol)
            .CreateModelMap(_mapService);
        
        //Assert
        Assert.Matches($@"Price = \(int\){AnyNamespace}Price".ReplaceSpaceWithAnySpace(),
            modelMap.CreateMappingSource(_mapService, SourceCreator.CreateSource("a")).Text);
    }
    
}