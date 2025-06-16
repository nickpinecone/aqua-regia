using System;
using AquaRegia.Library.Helpers;
using AquaRegia.World;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AquaRegia.Players;

public class SwimPlayer : ModPlayer
{
    public override void PostUpdateMiscEffects()
    {
        base.PostUpdateMiscEffects();

        if (Player.Center.Y / 16 < WorldConstants.FloodLevel) return;

        LightArea();
    }

    private void LightArea()
    {
        var area = ScreenRectangle();

        for (var x = area.Left; x < area.Right; x++)
        {
            for (var y = area.Top; y < area.Bottom; y++)
            {
                var tile = Main.tile[x, y];

                if (tile.LiquidType != LiquidID.Water || tile.LiquidAmount != 255 || TileHelper.IsSolid(tile)) continue;

                var lightAmount = (float)(1f - ((float)y - WorldConstants.FloodLevel) / Main.maxTilesY * 5);
                lightAmount = Math.Max(0, lightAmount);

                Lighting.AddLight(new Vector2(x * 16, y * 16), lightAmount, lightAmount, lightAmount);
            }
        }
    }

    private Rectangle ScreenRectangle()
    {
        var tileCoords = Main.screenPosition.ToTileCoordinates();

        var rect = new Rectangle(tileCoords.X - 5, tileCoords.Y - 5, (Main.ScreenSize.X / 16) + 10,
            (Main.ScreenSize.Y / 16) + 10);

        return rect;
    }
}