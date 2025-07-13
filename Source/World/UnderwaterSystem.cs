using System.Collections.Generic;
using AquaRegia.Library.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace AquaRegia.World;

public class UnderwaterSystem : ModSystem
{
    public static double TileSeaLevel => Main.worldSurface - (Main.worldSurface * 0.5f);

    public static LocalizedText? FloodGenMessage { get; private set; }

    public override void SetStaticDefaults()
    {
        FloodGenMessage = LocalizationHelper.GetLocalization($"World.{nameof(FloodGenMessage)}");
    }

    public static bool IsUnderwater(Point point)
    {
        return point.Y >= TileSeaLevel;
    }

    public static bool IsUnderwater(Vector2 position)
    {
        return IsUnderwater(position.ToTileCoordinates());
    }

    public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
    {
        // TODO also need to test this in other places, see how it behaves
        // the only weird thing right now is how plants interact with water
        // some break, others dont, once i remove them though, it's not gonna be a problem
        // so maybe this is an okay place for this pass
        WorldGenHelper.InsertAfterGenPass(tasks, "Final Cleanup", new FloodGenPass("Flood", 100f));
    }

    public override void PostUpdateEverything()
    {
        if (Main.netMode == NetmodeID.MultiplayerClient) return;

        foreach (var player in Main.ActivePlayers)
        {
            var area = new Rectangle(
                (int)(player.position.X / 16) - 75,
                (int)(player.position.Y / 16) - 50,
                150,
                100
            );

            FloodArea(area);
        }
    }

    private void FloodArea(Rectangle area)
    {
        area = ModHelper.ClampToTileWorld(area);

        for (var x = area.Left; x < area.Right; x++)
        {
            for (var y = area.Top; y < area.Bottom; y++)
            {
                var tile = Main.tile[x, y];

                if (!IsUnderwater(new Point(x, y)))
                {
                    tile.LiquidAmount = 0;
                    continue;
                }

                if (tile.LiquidAmount == 255) continue;

                tile.LiquidType = LiquidID.Water;
                tile.LiquidAmount = 255;
            }
        }
    }
}

public class FloodGenPass : GenPass
{
    public FloodGenPass(string name, double loadWeight) : base(name, loadWeight)
    {
    }

    protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
    {
        progress.Message = UnderwaterSystem.FloodGenMessage?.Value;

        var left = 0;
        var right = Main.maxTilesX;
        var top = UnderwaterSystem.TileSeaLevel;
        var bottom = Main.maxTilesY;

        for (var x = left; x < right; x++)
        {
            for (var y = (int)top; y < bottom; y++)
            {
                var tile = Main.tile[x, y];

                tile.LiquidType = LiquidID.Water;
                tile.LiquidAmount = 255;
            }
        }
    }
}