using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AquaRegia.Library;

public interface IModule
{
}

public interface IComposite<TRuntime>
{
    protected Dictionary<Type, IModule> Modules { get; }
    protected List<TRuntime> RuntimeModules { get; }

    public bool HasModule<T>()
    {
        return Modules.ContainsKey(typeof(T));
    }

    public void AddModule(params IModule[] modules)
    {
        foreach (var module in modules)
        {
            Modules[module.GetType()] = module;
        }
    }

    public bool TryAddModule(IModule module)
    {
        if (Modules.ContainsKey(module.GetType()))
        {
            return false;
        }

        AddModule(module);
        return true;
    }

    public T GetModule<T>()
        where T : IModule
    {
        return (T)Modules[typeof(T)];
    }

    public bool TryGetModule<T>([NotNullWhen(true)] out T? module)
        where T : IModule
    {
        if (HasModule<T>())
        {
            module = GetModule<T>();
            return true;
        }

        module = default(T);
        return false;
    }

    public void AddRuntimeModule(params TRuntime[] modules)
    {
        foreach (var module in modules)
        {
            RuntimeModules.Add(module);
        }
    }
}
