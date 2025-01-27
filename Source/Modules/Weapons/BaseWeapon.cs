using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace AquaRegia.Modules.Weapons;

public abstract class BaseWeapon : ModItem, IComposite<IWeaponRuntime>
{
    private Dictionary<Type, IModule> _modules = new();
    Dictionary<Type, IModule> IComposite<IWeaponRuntime>.Modules => _modules;
    private List<IWeaponRuntime> _runtime = new();
    List<IWeaponRuntime> IComposite<IWeaponRuntime>.RuntimeModules => _runtime;

    public IComposite<IWeaponRuntime> Composite { get; init; }

    protected BaseWeapon()
    {
        Composite = ((IComposite<IWeaponRuntime>)this);
    }

    public override bool AltFunctionUse(Player player)
    {
        base.AltFunctionUse(player);

        AltUseAlways(player);

        return false;
    }

    public void DoAltUse(Player player)
    {
        if (Main.mouseLeft && Main.mouseRight)
        {
            AltUseAlways(player);
        }
    }

    public virtual void AltUseAlways(Player player)
    {
    }
}
