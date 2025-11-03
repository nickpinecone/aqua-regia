using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
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

    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();

        foreach (var module in RuntimeModules)
        {
            module.RuntimeSetStaticDefaults(this);
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

    public override void HoldItem(Player player)
    {
        base.HoldItem(player);

        foreach (var module in RuntimeModules)
        {
            module.RuntimeHoldItem(this, player);
        }
    }
}