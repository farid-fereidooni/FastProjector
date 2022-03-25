using System;
using Xunit.Abstractions;

namespace FastProjector.Test.IocTests.TestServices;

public interface ITestService
{
    string GetId();
}

public class TestService: ITestService
{
    private readonly string _id;

    public TestService()
    {
        _id = Guid.NewGuid().ToString();
    }

    public string GetId()
    {
        return _id;
    }
}