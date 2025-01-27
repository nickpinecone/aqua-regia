using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AquaRegia.Modules;

public interface IComposite<TRuntime>
{
    protected Dictionary<Type, IModule> Modules { get; }
    protected List<TRuntime> RuntimeModules { get; }

    public bool HasModule<T>()
    {
        return Modules.ContainsKey(typeof(T));
    }

    protected void AddModule(params IModule[] modules)
    {
        foreach (var module in modules)
        {
            Modules[module.GetType()] = module;
        }
    }

    protected bool TryAddModule(IModule module)
    {
        if (Modules.ContainsKey(module.GetType()))
        {
            return false;
        }
        else
        {
            AddModule(module);
            return true;
        }
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
        else
        {
            module = default(T);
            return false;
        }
    }

    public void AddRuntimeModule(params TRuntime[] modules)
    {
        foreach (var module in modules)
        {
            RuntimeModules.Add(module);
        }
    }
}
