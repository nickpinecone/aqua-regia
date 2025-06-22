using System;
using AquaRegia.Library.Helpers;
using AquaRegia.World;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AquaRegia.Players;

// TODO refactor into a LightingSystem
public class SwimPlayer : ModPlayer
{
    public override void PostUpdateMiscEffects()
    {
        base.PostUpdateMiscEffects();

        if (Player.Center.Y / 16 < WorldConstants.FloodLevel) return;

        LightArea();
    }

    float GetVanillaBrightnessScale()
    {
        if (Main.dayTime)
        {
            var normalizedTime = (float)(Main.time / Main.dayLength);
            return MathHelper.Lerp(0.5f, 1.0f, Math.Abs(normalizedTime - 0.5f) * -2f + 1f);
        }
        else
        {
            var normalizedTime = (float)(Main.time / Main.nightLength);
            return MathHelper.Lerp(0.5f, 0.2f, Math.Abs(normalizedTime - 0.5f) * -2f + 1f);
        }
    }

    // Maybe to optimise i'll actually hook into a lighting check
    private void LightArea()
    {
        var area = ScreenRectangle();
        const int depth = 200;
        const float throughSolid = 20f;

        for (var x = area.Left; x < area.Right; x++)
        {
            var startY = (int)WorldConstants.FloodLevel;
            var lightStrength = GetVanillaBrightnessScale();

            for (var y = 0; y < depth; y++)
            {
                // How much solid tiles will impact light strength, the deeper we are the more
                var solidImpact = ((float)y / depth) * (throughSolid / depth);
                var tile = Main.tile[x, startY + y];

                if (TileHelper.IsSolid(tile) && Main.tileBlockLight[tile.TileType])
                {
                    lightStrength -= solidImpact;
                }
                else
                {
                    // Natural light fall off
                    lightStrength -= 1f / depth;

                    Lighting.AddLight(new Vector2(x * 16, (startY + y) * 16), lightStrength, lightStrength,
                        lightStrength);
                }

                if (lightStrength <= 0.01f)
                {
                    break;
                }
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