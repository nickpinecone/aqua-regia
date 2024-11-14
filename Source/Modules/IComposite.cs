using System;
using System.Collections.Generic;

namespace AquaRegia.Modules;

public interface IComposite<TModule>
{
    protected Dictionary<Type, TModule> Modules { get; }
    protected List<TModule> RuntimeModules { get; }

    public bool HasModule<T>()
    {
        return Modules.ContainsKey(typeof(T));
    }

    public void AddModule(TModule module)
    {
        Modules[module.GetType()] = module;
    }

    public void AddRuntimeModule(TModule module)
    {
        if (!Modules.ContainsKey(module.GetType()))
        {
            AddModule(module);
            RuntimeModules.Add(module);
        }
    }

    public T GetModule<T>()
        where T : TModule
    {
        return (T)Modules[typeof(T)];
    }
}
