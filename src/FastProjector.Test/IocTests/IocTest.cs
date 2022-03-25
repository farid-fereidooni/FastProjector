using FastProjector.Ioc;
using FastProjector.Models;
using FastProjector.Test.Helpers;
using FastProjector.Test.IocTests.TestServices;
using SourceCreationHelper.Core;
using Xunit;

namespace FastProjector.Test.IocTests;

public class IocTest
{
    private readonly IContainer _container;
    
    public IocTest()
    {
        _container = new IocContainer();
    }
    [Fact]
    public void GetService_InjectingTransientSeveralTime_ReturnsDifferentInstances()
    {
        //Arrange
        _container.AddTransient<ITestService>(c => new TestService());

        var scope = _container.CreateScope();
        
        //Act
        var id1 = scope.GetService<ITestService>().GetId();
        var id2 = scope.GetService<ITestService>().GetId();
        
        //Assert
       Assert.NotEqual(id1, id2);

    }
    
    [Fact]
    public void GetService_InjectingScopedInSameScope_ReturnsSameInstance()
    {
        //Arrange
        _container.AddScoped<ITestService>(c => new TestService());

        var scope = _container.CreateScope();
        
        //Act
        var id1 = scope.GetService<ITestService>().GetId();
        var id2 = scope.GetService<ITestService>().GetId();
        
        //Assert
        Assert.Equal(id1, id2);

    }
    
    [Fact]
    public void GetService_InjectingScopedInDifferentScopes_ReturnsDifferentInstances()
    {
        //Arrange
        _container.AddScoped<ITestService>(c => new TestService());

        
        //Act
        var scope1 = _container.CreateScope();
        var id1 = scope1.GetService<ITestService>().GetId();
        var scope2 = _container.CreateScope();
        var id2 = scope2.GetService<ITestService>().GetId();
        
        //Assert
        Assert.NotEqual(id1, id2);

    }
    
    
    [Fact]
    public void GetService_InjectingSingletonInDifferentScopes_ReturnsSameInstances()
    {
        //Arrange
        _container.AddSingleton<ITestService>(c => new TestService());

        
        //Act
        var scope1 = _container.CreateScope();
        var id1 = scope1.GetService<ITestService>().GetId();
        var id2 = scope1.GetService<ITestService>().GetId();
        var scope2 = _container.CreateScope();
        var id3 = scope2.GetService<ITestService>().GetId();
        
        //Assert
        Assert.Equal(id1, id2);
        Assert.Equal(id1, id3);

    }
    
    [Fact]
    public void GetService_InjectingTransientWithSingleton_ReturnsSameSingletonButDifferentTransient()
    {
        //Arrange
        _container.AddSingleton<ITestService>(c => new TestService());
        _container.AddTransient<INestedTestService>(c => new NestedTestService(c.GetService<ITestService>()));

        
        //Act
        var scope1 = _container.CreateScope();
        var transientService1 = scope1.GetService<INestedTestService>();
        var transientId1 = transientService1.GetId();
        var singletonId1 = transientService1.GetDependentId();
        
        var transientService2 = scope1.GetService<INestedTestService>();
        var transientId2 = transientService2.GetId();
        var singletonId2 = transientService2.GetDependentId();

        
        //Assert
        Assert.Equal(singletonId1, singletonId2);
        Assert.NotEqual(transientId1, transientId2);

    }
}