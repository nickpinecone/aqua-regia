using AquaRegia.Library.Extended;
using AquaRegia.Library.Extended.Modules.Attributes;

namespace Tests;

public interface ITestItemRuntime;

public class TestModule : IModule, ITestItemRuntime
{
}

public class Test2Module : IModule, ITestItemRuntime
{
}

public class TestItem : IComposite<ITestItemRuntime>
{
    public Dictionary<Type, IModule> Modules { get; } = [];
    public List<ITestItemRuntime> RuntimeModules { get; } = [];

    public TestItem()
    {
        ((IComposite<ITestItemRuntime>)this).AddModules();
    }

    [RuntimeModule] private TestModule Test { get; } = new();
    [RuntimeModule(1)] private Test2Module Test2 { get; } = new();
}

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        var testItem = new TestItem();
        Assert.That(testItem.RuntimeModules, Is.Not.Empty);

        var firstModule = testItem.RuntimeModules.FindIndex(x => x is TestModule);
        var secondModule = testItem.RuntimeModules.FindIndex(x => x is Test2Module);
        Assert.That(secondModule, Is.GreaterThan(firstModule));
    }
}