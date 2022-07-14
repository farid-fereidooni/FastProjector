using FastProjector.Contracts;
using FastProjector.Models;
using FastProjector.Models.Casting;
using FastProjector.Models.TypeMetaDatas;
using FastProjector.Repositories;
using FastProjector.Services;
using FastProjector.Test.Helpers;
using NSubstitute;
using SourceCreationHelper;
using SourceCreationHelper.Core;
using Xunit;
using static FastProjector.Test.Helpers.RegexHelper;

namespace FastProjector.Test.PropertyMappingTests;

public class BindTest
{
    private readonly ModelMapService _mapService;

    public BindTest()
    {
        _mapService = new ModelMapService(new MapRepository(),
            new CastingService(DefaultCastingConfigurations.GetConfigurations()), new VariableNameGenerator());
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
        var modelMap = new ModelMapMetaData(productSymbol, productModelSymbol)
            .CreateModelMap(_mapService);
        var sourceText = modelMap.CreateMappingSource(_mapService, SourceCreator.CreateSource("a")).Text;

        //Assert
        Assert.Matches(@"Name\s*=\s*a\.Name", sourceText);
        Assert.Matches(@"Price\s*=\s*a\.Price", sourceText);
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
        var source = new ModelMapMetaData(productSymbol, productModelSymbol)
            .CreateModelMap(_mapService)
            .CreateMappingSource(_mapService, SourceCreator.CreateSource("a"));

        //Assert
        Assert.DoesNotMatch(@"Price\s*=\s*a\.Price", source.Text);
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
        var categorySymbol = compilation.GetClassSymbol("Category");

        var categoryModelMap = new ModelMapMetaData(categorySymbol, categorySymbol).CreateModelMap(_mapService);

        var mapResolverMock = Substitute.For<IMapResolverService>();
        mapResolverMock.ResolveMap(Arg.Is<ClassTypeMetaData>(a => a.TypeInformation.FullName.Contains("Category")),
                Arg.Is<ClassTypeMetaData>(x => x.TypeInformation.FullName.Contains("Category")))
            .Returns(categoryModelMap);

        //Act
        var modelMap = new ModelMapMetaData(productSymbol, productModelSymbol)
            .CreateModelMap(_mapService);

        modelMap.TryResolveRequiredMaps(mapResolverMock);

        var source = modelMap.CreateMappingSource(_mapService, SourceCreator.CreateSource("a"));

        //Assert
        Assert.Matches(
            $@"Category = new {AnyNamespace}Category 
                                            {{ 
                                                {Anything}
                                                Name = {AnyNamespace}Name
                            ".ReplaceSpaceWithAnySpace()
            , source.Text);
    }
}