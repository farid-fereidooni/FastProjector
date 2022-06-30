using System.Linq;
using FastProjector.Contracts;
using FastProjector.Helpers;
using FastProjector.Models;
using FastProjector.Models.Casting;
using FastProjector.Models.TypeInformations;
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
    private readonly ICastingService _castingService;

    public CastTest()
    {
        _castingService = new CastingService(DefaultCastingConfigurations.GetConfigurations());
        _mapService = new ModelMapService(new MapCache(),_castingService , new VariableNameGenerator());
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
    
    
     
    [Theory]
    [InlineData("IEnumerable<int>")]
    [InlineData("int[]")]
    [InlineData("List<int>")]
    [InlineData("ICollection<int>")]
    [InlineData("IList<int>")]
    [InlineData("HashSet<int>")]
    public void CastType_CastAnyCollectionToList_IsSuccessful(string fromCollectionType)
    {
        //Arranges
        var sourceBuilder = new SourceBuilder();
        sourceBuilder.AddClass("Test")
            .AddProperty(AccessModifier.@public, fromCollectionType, "ListA")
            .AddProperty(AccessModifier.@public, "List<int>", "ListB");

        var compilation = sourceBuilder.GetCompilation();
        
        var properties = compilation.GetClassSymbol("Test")
            .ExtractProps().ToList();
        
        var listAType = TypeInformation.Create(properties.First());
        var listBType = TypeInformation.Create(properties.Last());
        
        //Act
        var castResult = _castingService.CastType(listAType, listBType);

        //Assert
        Assert.False(castResult.IsUnMapable);
        Assert.Equal("any.ToList()",castResult.Cast("any"));
    }
    
    [Theory]
    [InlineData("IEnumerable<int>")]
    [InlineData("int[]")]
    [InlineData("List<int>")]
    [InlineData("ICollection<int>")]
    [InlineData("IList<int>")]
    [InlineData("HashSet<int>")]
    public void CastType_CastAnyCollectionToArray_IsSuccessful(string fromCollectionType)
    {
        //Arranges
        var sourceBuilder = new SourceBuilder();
        sourceBuilder.AddClass("Test")
            .AddProperty(AccessModifier.@public, fromCollectionType, "ListA")
            .AddProperty(AccessModifier.@public, "int[]", "ListB");

        var compilation = sourceBuilder.GetCompilation();
        
        var properties = compilation.GetClassSymbol("Test")
            .ExtractProps().ToList();
        
        var listAType = TypeInformation.Create(properties.First());
        var listBType = TypeInformation.Create(properties.Last());
        
        //Act
        var castResult = _castingService.CastType(listAType, listBType);

        //Assert
        Assert.False(castResult.IsUnMapable);
        Assert.Equal("any.ToArray()",castResult.Cast("any"));
    }
    
    
    [Theory]
    [InlineData("IEnumerable<int>")]
    [InlineData("int[]")]
    [InlineData("List<int>")]
    [InlineData("ICollection<int>")]
    [InlineData("IList<int>")]
    [InlineData("HashSet<int>")]
    public void CastType_CastAnyCollectionToEnumerable_IsSuccessful(string fromCollectionType)
    {
        //Arranges
        var sourceBuilder = new SourceBuilder();
        sourceBuilder.AddClass("Test")
            .AddProperty(AccessModifier.@public, fromCollectionType, "ListA")
            .AddProperty(AccessModifier.@public, "IEnumerable<int>", "ListB");

        var compilation = sourceBuilder.GetCompilation();
        
        var properties = compilation.GetClassSymbol("Test")
            .ExtractProps().ToList();
        
        var listAType = TypeInformation.Create(properties.First());
        var listBType = TypeInformation.Create(properties.Last());
        
        //Act
        var castResult = _castingService.CastType(listAType, listBType);

        //Assert
        Assert.False(castResult.IsUnMapable);
        Assert.Equal("any.ToList()",castResult.Cast("any"));
    }
}