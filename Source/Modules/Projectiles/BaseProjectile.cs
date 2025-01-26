using System;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace AquaRegia.Modules.Projectiles;

public abstract class BaseProjectile : ModProjectile, IComposite<IProjectileRuntime>
{
    private Dictionary<Type, IModule> _modules = new();
    Dictionary<Type, IModule> IComposite<IProjectileRuntime>.Modules => _modules;
    private List<IProjectileRuntime> _runtime = new();
    List<IProjectileRuntime> IComposite<IProjectileRuntime>.RuntimeModules => _runtime;
    protected IComposite<IProjectileRuntime> _composite;

    protected BaseProjectile()
    {
        _composite = ((IComposite<IProjectileRuntime>)this);
    }
}
