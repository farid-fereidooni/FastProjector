using System;

namespace FastProjector.Test.IocTests.TestServices;

public interface INestedTestService
{
    string GetId();
    string GetDependentId();
}

public class NestedTestService: INestedTestService
{
    private readonly ITestService _testService;
    private readonly string _id;

    public NestedTestService(ITestService testService)
    {
        _testService = testService;
        _id = Guid.NewGuid().ToString();
    }

    public string GetId()
    {
        return _id;
    }

    public string GetDependentId()
    {
        return _testService.GetId();
    }
}