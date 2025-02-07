using System;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace AquaRegia.Modules.Mobs;

public abstract class BaseMob : ModNPC, IComposite<IMobRuntime>
{
    private Dictionary<Type, IModule> _modules = new();
    Dictionary<Type, IModule> IComposite<IMobRuntime>.Modules => _modules;
    private List<IMobRuntime> _runtime = new();
    List<IMobRuntime> IComposite<IMobRuntime>.RuntimeModules => _runtime;

    public IComposite<IMobRuntime> Composite { get; init; }

    public BaseMob()
    {
        Composite = ((IComposite<IMobRuntime>)this);
    }
}
