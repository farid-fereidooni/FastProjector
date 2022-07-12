using System;
using FastProjector.Contracts;
using FastProjector.Models;
using FastProjector.Models.Casting;
using FastProjector.Models.TypeInformations;
using FastProjector.Models.TypeMetaDatas;
using FastProjector.Services;
using FastProjector.Test.Helpers;
using NSubstitute;
using NSubstitute.Extensions;
using SourceCreationHelper;
using SourceCreationHelper.Core;
using SourceCreationHelper.Interfaces;
using Xunit;
using static FastProjector.Test.Helpers.RegexHelper;

namespace FastProjector.Test.PropertyMappingTests;

public class ProjectionTest
{
    private readonly ModelMapService _mapService;
    private readonly ICastingService _castingService;
    private readonly IVariableNameGenerator _variableNameGenerator;

    public ProjectionTest()
    {
        _variableNameGenerator = new VariableNameGenerator();
        _castingService = new CastingService(DefaultCastingConfigurations.GetConfigurations());
        _mapService = new ModelMapService(new MapCache(), _castingService, _variableNameGenerator);
    }

    [Theory]
    [InlineData("List<int>")]
    [InlineData("IEnumerable<int>")]
    [InlineData("HashSet<int>")]
    [InlineData("int[]")]
    public void CreateMappingSource_PrimitiveProjectionToListType_IsSuccessful(string sourcePropertyType)
    {
        //Arrange
        var sourceBuilder = new SourceBuilder();
        sourceBuilder.AddClass("User")
            .AddProperty(AccessModifier.@public, sourcePropertyType, "Roles");

        sourceBuilder.AddClass("UserModel")
            .AddProperty(AccessModifier.@public, "List<int>", "Roles");

        var compilation = sourceBuilder.GetCompilation();

        var userSymbol = compilation.GetClassSymbol("User");
        var userModelSymbol = compilation.GetClassSymbol("UserModel");

        //Act
        var modelMap = new ModelMapMetaData(userSymbol, userModelSymbol)
            .CreateModelMap(_mapService);
        var sourceText = modelMap.CreateMappingSource(_mapService, SourceCreator.CreateSource("a")).Text;

        //Assert
        Assert.Matches(@$"Roles = a\.Roles.ToList\(\)", sourceText);
    }

    [Theory]
    [InlineData("List<int>")]
    [InlineData("IEnumerable<int>")]
    [InlineData("HashSet<int>")]
    [InlineData("int[]")]
    public void CreateMappingSource_PrimitiveProjectionToArrayType_IsSuccessful(string sourcePropertyType)
    {
        //Arrange
        var sourceBuilder = new SourceBuilder();
        sourceBuilder.AddClass("User")
            .AddProperty(AccessModifier.@public, sourcePropertyType, "Roles");

        sourceBuilder.AddClass("UserModel")
            .AddProperty(AccessModifier.@public, "int[]", "Roles");

        var compilation = sourceBuilder.GetCompilation();

        var userSymbol = compilation.GetClassSymbol("User");
        var userModelSymbol = compilation.GetClassSymbol("UserModel");

        //Act
        var modelMap = new ModelMapMetaData(userSymbol, userModelSymbol)
            .CreateModelMap(_mapService);
        var sourceText = modelMap.CreateMappingSource(_mapService, SourceCreator.CreateSource("a")).Text;

        //Assert
        Assert.Matches(@"Roles = a\.Roles.ToArray\(\)".ReplaceSpaceWithAnySpace(), sourceText);
    }


    [Fact]
    public void CreateMappingSource_ClassProjection_IsSuccessful()
    {
        //Arrange
        var sourceBuilder = new SourceBuilder();

        sourceBuilder.AddClass("Role")
            .AddProperty(AccessModifier.@public, "string", "Name");

        sourceBuilder.AddClass("RoleModel")
            .AddProperty(AccessModifier.@public, "string", "Name");

        sourceBuilder.AddClass("User")
            .AddProperty(AccessModifier.@public, "List<Role>", "Roles");

        sourceBuilder.AddClass("UserModel")
            .AddProperty(AccessModifier.@public, "List<RoleModel>", "Roles");

        var compilation = sourceBuilder.GetCompilation();

        var userSymbol = compilation.GetClassSymbol("User");
        var userModelSymbol = compilation.GetClassSymbol("UserModel");
        var roleSymbol = compilation.GetClassSymbol("Role");
        var roleModelSymbol = compilation.GetClassSymbol("RoleModel");

        var roleModelMap = new ModelMapMetaData(roleSymbol, roleModelSymbol).CreateModelMap(_mapService);

        var mapCacheMock = Substitute.For<IMapCache>();
        mapCacheMock.Get(Arg.Is<TypeInformation>(a => a.FullName.Contains("Role")),
                Arg.Is<TypeInformation>(x => x.FullName.Contains("RoleModel")))
            .Returns(roleModelMap);

        var mapResolverMock = Substitute.For<IMapResolverService>();
        mapResolverMock.ResolveMap(Arg.Is<ClassTypeMetaData>(a => a.TypeInformation.FullName.Contains("Role")),
                Arg.Is<ClassTypeMetaData>(x => x.TypeInformation.FullName.Contains("RoleModel")))
            .Returns(roleModelMap);

        var mapServiceMock = new ModelMapService(mapCacheMock, _castingService, _variableNameGenerator);

        //Act
        var modelMap = new ModelMapMetaData(userSymbol, userModelSymbol)
            .CreateModelMap(_mapService);
        modelMap.TryResolveRequiredMaps(mapResolverMock);
        var sourceText = modelMap.CreateMappingSource(mapServiceMock, SourceCreator.CreateSource("a")).Text;

        //Assert
        Assert.Matches(
            $@"Roles = a\.Roles\.Select\(\({Anything}\) => new {AnyNamespace}RoleModel {{ Name = {AnyNamespace}Name }} \)"
                .ReplaceSpaceWithAnySpace(),
            sourceText);
    }

    [Fact]
    public void CreateMappingSource_NestedPrimitiveProjection_IsSuccessful()
    {
        //Arrange
        var sourceBuilder = new SourceBuilder();
        sourceBuilder.AddClass("User")
            .AddProperty(AccessModifier.@public, "List<List<List<int>>>", "Roles");

        sourceBuilder.AddClass("UserModel")
            .AddProperty(AccessModifier.@public, "List<List<List<int>>>", "Roles");

        var compilation = sourceBuilder.GetCompilation();

        var userSymbol = compilation.GetClassSymbol("User");
        var userModelSymbol = compilation.GetClassSymbol("UserModel");

        //Act
        var modelMap = new ModelMapMetaData(userSymbol, userModelSymbol)
            .CreateModelMap(_mapService);
        var sourceText = modelMap.CreateMappingSource(_mapService, SourceCreator.CreateSource("a")).Text;

        //Assert
        Assert.Matches(
            @$"Roles = a\.Roles\.Select\(\({Anything}\) => {AnyNamespace}Select\(\({Anything} => {AnyNamespace}ToList\(\) \)\.ToList\(\) \)\.ToList\(\)"
                .ReplaceSpaceWithAnySpace(),
            sourceText);
    }
    
      [Fact]
    public void CreateMappingSource_NestedClassProjection_IsSuccessful()
    {
        //Arrange
        var sourceBuilder = new SourceBuilder();

        sourceBuilder.AddClass("Role")
            .AddProperty(AccessModifier.@public, "string", "Name");

        sourceBuilder.AddClass("RoleModel")
            .AddProperty(AccessModifier.@public, "string", "Name");

        sourceBuilder.AddClass("User")
            .AddProperty(AccessModifier.@public, "List<List<List<Role>>>", "Roles");

        sourceBuilder.AddClass("UserModel")
            .AddProperty(AccessModifier.@public, "List<List<List<RoleModel>>>", "Roles");

        var compilation = sourceBuilder.GetCompilation();

        var userSymbol = compilation.GetClassSymbol("User");
        var userModelSymbol = compilation.GetClassSymbol("UserModel");
        var roleSymbol = compilation.GetClassSymbol("Role");
        var roleModelSymbol = compilation.GetClassSymbol("RoleModel");

        var roleModelMap = new ModelMapMetaData(roleSymbol, roleModelSymbol).CreateModelMap(_mapService);

        var mapCacheMock = Substitute.For<IMapCache>();
        mapCacheMock.Get(Arg.Is<TypeInformation>(a => a.FullName.Contains("Role")),
                Arg.Is<TypeInformation>(x => x.FullName.Contains("RoleModel")))
            .Returns(roleModelMap);

        var mapResolverMock = Substitute.For<IMapResolverService>();
        mapResolverMock.ResolveMap(Arg.Is<ClassTypeMetaData>(a => a.TypeInformation.FullName.Contains("Role")),
                Arg.Is<ClassTypeMetaData>(x => x.TypeInformation.FullName.Contains("RoleModel")))
            .Returns(roleModelMap);

        var mapServiceMock = new ModelMapService(mapCacheMock, _castingService, _variableNameGenerator);


        //Act
        var modelMap = new ModelMapMetaData(userSymbol, userModelSymbol)
            .CreateModelMap(_mapService);

        modelMap.TryResolveRequiredMaps(mapResolverMock);
        var sourceText = modelMap.CreateMappingSource(mapServiceMock, SourceCreator.CreateSource("a")).Text;

        //Assert
        Assert.Matches(
            @$"Roles = a\.Roles\.Select\(\({Anything}\) => {AnyNamespace}Select\(\({Anything} =>  new {AnyNamespace}RoleModel {{ Name = {AnyNamespace}Name }} \)\.ToList\(\) \)\.ToList\(\)"
                .ReplaceSpaceWithAnySpace(),
            sourceText);
    }
}