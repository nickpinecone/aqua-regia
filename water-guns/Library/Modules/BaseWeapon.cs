using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace WaterGuns.Library.Modules;

public abstract class BaseWeapon : ModItem, IComposite<IWeaponRuntime>
{
    public Dictionary<Type, IModule> Modules => new();
    public List<IWeaponRuntime> RuntimeModules => new();

    public IComposite<IWeaponRuntime> Composite { get; init; }

    protected BaseWeapon()
    {
        Composite = this;
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

    public override Vector2? HoldoutOffset()
    {
        var defaultValue = base.HoldoutOffset();
        Vector2? custom = null;

        foreach (var module in RuntimeModules)
        {
            var value = module.RuntimeHoldoutOffset(this);

            if (value != defaultValue)
            {
                custom = value;
            }
        }

        return custom ?? defaultValue;
    }

    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        base.ModifyTooltips(tooltips);

        foreach (var module in RuntimeModules)
        {
            module.RuntimeModifyTooltips(this, tooltips);
        }
    }
}

public interface IWeaponRuntime
{
    public void RuntimeModifyTooltips(BaseWeapon weapon, List<TooltipLine> tooltip)
    {
    }

    public Vector2? RuntimeHoldoutOffset(BaseWeapon weapon)
    {
        return null;
    }
}