using System;
using System.Collections.Generic;
using AquaRegia.Library.Extended.Modules.Sources;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AquaRegia.Library.Extended.Modules;

public abstract class BaseItem : ModItem, IComposite<IItemRuntime>
{
    public Dictionary<Type, IModule> Modules { get; } = [];
    public List<IItemRuntime> RuntimeModules { get; } = [];

    public IComposite<IItemRuntime> Composite { get; init; }

    protected BaseItem()
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

    public virtual bool PreShoot(out WeaponWithAmmoSource weaponSource, Player player,
        EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        weaponSource = new WeaponWithAmmoSource(source, this);
        return true;
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity,
        int type, int damage, float knockback)
    {
        base.Shoot(player, source, position, velocity, type, damage, knockback);

        if (PreShoot(out var weaponSource, player, source, position, velocity, type, damage, knockback))
        {
            Projectile.NewProjectileDirect(weaponSource, position, velocity, Item.shoot, damage, knockback,
                player.whoAmI);
        }

        return false;
    }

    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();

        foreach (var module in RuntimeModules)
        {
            module.RuntimeSetStaticDefaults(this);
        }
    }

    public override void HoldItem(Player player)
    {
        base.HoldItem(player);

        DoAltUse(player);

        foreach (var module in RuntimeModules)
        {
            module.RuntimeHoldItem(this, player);
        }
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

    public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type,
        ref int damage,
        ref float knockback)
    {
        base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);

        foreach (var module in RuntimeModules)
        {
            module.RuntimeModifyShootStats(this, player, ref position, ref velocity, ref type, ref damage,
                ref knockback);
        }
    }
}