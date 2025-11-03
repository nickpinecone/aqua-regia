using System;
using AquaRegia.Library.Extended.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AquaRegia.World;

public class LightingSystem : ModSystem
{
    public float VisionDay { get; set; } = 1f;
    public float VisionTwilight { get; set; } = 0.5f;
    public float VisionNight { get; set; } = 0.2f;

    public const int TileDepth = 200;
    public const float DecayThroughSolid = 20f;

    public override void PostDrawTiles()
    {
        base.PostDrawTiles();

        if (UnderwaterSystem.IsUnderwater(Main.LocalPlayer.Center))
        {
            LightArea();
        }
    }

    // TODO account for potions (night vision), events (blood moon) and potentially other stuff
    private float GetBrightnessScale()
    {
        if (Main.dayTime)
        {
            var normalizedTime = (float)(Main.time / Main.dayLength);
            return MathHelper.Lerp(VisionTwilight, VisionDay, Math.Abs(normalizedTime - 0.5f) * -2f + 1f);
        }
        else
        {
            var normalizedTime = (float)(Main.time / Main.nightLength);
            return MathHelper.Lerp(VisionTwilight, VisionNight, Math.Abs(normalizedTime - 0.5f) * -2f + 1f);
        }
    }

    private void LightArea()
    {
        var area = ModHelper.GetScreenTileRectangle();
        var lightStrength = GetBrightnessScale();

        // Optimization to skip light calculation if we are below the possible lighted area
        if (area.Top > UnderwaterSystem.TileSeaLevel + (TileDepth * lightStrength))
        {
            return;
        }

        for (var x = area.Left; x < area.Right; x++)
        {
            var startY = (int)UnderwaterSystem.TileSeaLevel;
            lightStrength = GetBrightnessScale();

            for (var y = 0; y < TileDepth; y++)
            {
                var tile = Main.tile[x, startY + y];

                if (TileHelper.IsSolid(tile) && Main.tileBlockLight[tile.TileType])
                {
                    lightStrength -= ((float)y / TileDepth) * (DecayThroughSolid / TileDepth);
                }
                else
                {
                    lightStrength -= 1f / TileDepth;

                    Lighting.AddLight(
                        new Vector2(x * 16, (startY + y) * 16),
                        lightStrength, lightStrength, lightStrength
                    );
                }

                if (lightStrength <= 0.01f)
                {
                    break;
                }
            }
        }
    }
}