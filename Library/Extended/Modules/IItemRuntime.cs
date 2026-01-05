using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AquaRegia.Library.Extended.Modules;

public interface IItemRuntime
{
    public void RuntimeSetStaticDefaults(BaseItem baseItem)
    {
    }

    public void RuntimeModifyTooltips(BaseItem baseItem, List<TooltipLine> tooltip)
    {
    }

    public Vector2? RuntimeHoldoutOffset(BaseItem baseItem)
    {
        return null;
    }

    public void RuntimeHoldItem(BaseItem baseItem, Player player)
    {
    }

    public void RuntimeModifyShootStats(BaseItem baseItem, Player player, ref Vector2 position,
        ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
    {
    }

    public bool RuntimeCanUseItem(BaseItem baseItem, Player player)
    {
        return true;
    }

    public bool? RuntimeUseItem(BaseItem baseItem, Player player)
    {
        return null;
    }

    void RuntimeAltUseAlways(BaseItem baseItem, Player player)
    {
    }

    void RuntimeSetDefaults(BaseItem baseItem)
    {
    }
}