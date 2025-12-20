using System;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace AquaRegia.Library.Extended.Modules;

public abstract class BaseAmmo : ModItem, IComposite<IItemRuntime>
{
    public Dictionary<Type, IModule> Modules { get; } = [];
    public List<IItemRuntime> RuntimeModules { get; } = [];

    public IComposite<IItemRuntime> Composite { get; init; }

    protected BaseAmmo()
    {
        Composite = this;
    }

    public virtual void ApplyToProjectile(BaseProjectile projectile)
    {
    }
}