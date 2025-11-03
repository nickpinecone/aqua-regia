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
}