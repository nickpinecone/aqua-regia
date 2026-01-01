using AquaRegia.Library.Extended;

namespace Tests.Modules;

public class BaseTestItem : IComposite<ITestItemRuntime>
{
    public Dictionary<Type, IModule> Modules { get; } = [];
    public List<ITestItemRuntime> RuntimeModules { get; } = [];

    public IComposite<ITestItemRuntime> Composite { get; }

    public BaseTestItem()
    {
        Composite = this;
        Composite.AddModules();
    }
}