using System;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace AquaRegia.Modules.Weapons;

public abstract class BaseWeapon : ModItem, IComposite<IWeaponRuntime>
{
    private Dictionary<Type, IModule> _modules = new();
    Dictionary<Type, IModule> IComposite<IWeaponRuntime>.Modules => _modules;
    private List<IWeaponRuntime> _runtime = new();
    List<IWeaponRuntime> IComposite<IWeaponRuntime>.RuntimeModules => _runtime;
    protected IComposite<IWeaponRuntime> _composite;

    protected BaseWeapon()
    {
        _composite = ((IComposite<IWeaponRuntime>)this);
    }
}
