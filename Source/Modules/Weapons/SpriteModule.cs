using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using AquaRegia.Ammo;

namespace AquaRegia.Modules.Weapons;

public class SpriteModule : BaseGunModule
{
    public Vector2 Offset { get; set; }
    public Vector2 Shift { get; set; }

    public Vector2 HoldoutOffset { get; set; }

    public SpriteModule(BaseGun baseGun) : base(baseGun)
    {
    }

    public void AddAmmoTooltip(List<TooltipLine> tooltip, Mod mod)
    {
        tooltip.Add(new TooltipLine(mod, "AmmoUse",
                                    $"[c/9CCFD8:Uses Bottled Water] [i/s1:{ModContent.ItemType<BottledWater>()}]"));
    }

    public Vector2 ApplyOffset(Vector2 position, Vector2 velocity)
    {
        var normalized = velocity.SafeNormalize(Vector2.Zero);

        var offset = new Vector2(position.X + normalized.X * Offset.X, position.Y + normalized.Y * Offset.Y);

        return offset + Shift;
    }
}
