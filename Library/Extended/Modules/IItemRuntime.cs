using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AquaRegia.Library.Extended.Modules;

public interface IItemRuntime
{
    public void RuntimeSetStaticDefaults(BaseItem item)
    {
    }

    public void RuntimeModifyTooltips(BaseItem item, List<TooltipLine> tooltip)
    {
    }

    public Vector2? RuntimeHoldoutOffset(BaseItem item)
    {
        return null;
    }

    public void RuntimeHoldItem(BaseItem item, Player player)
    {
    }

    public void RuntimeModifyShootStats(BaseItem item, Player player, ref Vector2 position,
        ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
    {
    }

    public bool RuntimeCanUseItem(BaseItem item, Player player)
    {
        return true;
    }

    public bool? RuntimeUseItem(BaseItem item, Player player)
    {
        return null;
    }

    void RuntimeAltUseAlways(BaseItem item, Player player)
    {
    }
}