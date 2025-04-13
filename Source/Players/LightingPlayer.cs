using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AquaRegia.Source.World;

namespace AquaRegia.Source.Players;

public class LightingPlayer : ModPlayer
{
    public override void PostUpdateMiscEffects()
    {
        base.PostUpdateMiscEffects();

        if (Player.wet)
        {
            if (Player.active)
            {
                Player.gravity = 0;

                Rectangle area = new Rectangle(
                    (int)(Player.position.X / 16) - 100,
                    (int)(Player.position.Y / 16) - 100,
                    200,
                    200
                );

                LightArea(area);
            }
        }
    }

    public static bool IsSolid(Tile tile, bool alsoSolidTop = false)
    {
        if (alsoSolidTop)
        {
            return tile.HasTile && Main.tileSolid[tile.TileType];
        }
        else
        {
            return tile.HasTile && Main.tileSolid[tile.TileType] && !Main.tileSolidTop[tile.TileType];
        }
    }

    private void LightArea(Rectangle area)
    {
        area = ClampToWorld(area);

        for (int x = area.Left; x < area.Right; x++)
        {
            for (int y = area.Top; y < area.Bottom; y++)
            {
                var tile = Main.tile[x, y];

                if (tile.LiquidType == LiquidID.Water && tile.LiquidAmount == 255 && !IsSolid(tile))
                {
                    float lightAmount = 1f - ((float)y - FloodSystem.FloodLevel) / Main.maxTilesY * 5;
                    lightAmount = Math.Max(0, lightAmount);

                    Lighting.AddLight(new Vector2(x * 16, y * 16), lightAmount, lightAmount, lightAmount);
                }
            }
        }
    }

    private Rectangle ClampToWorld(Rectangle area)
    {
        area.X = (int)MathHelper.Clamp(area.X, 0, Main.maxTilesX);
        area.Y = (int)MathHelper.Clamp(area.Y, 0, Main.maxTilesY);
        return area;
    }
}