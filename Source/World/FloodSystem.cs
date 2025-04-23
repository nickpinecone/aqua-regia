using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace AquaRegia.World;

public class FloodSystem : ModSystem
{
    public static LocalizedText? FloodGenMessage { get; private set; }

    public static int FloodLevel => (int)(Main.worldSurface - Main.worldSurface * 0.5f);

    public override void SetStaticDefaults()
    {
        FloodGenMessage = Language.GetOrRegister(Mod.GetLocalizationKey($"WorldGen.{nameof(FloodGenMessage)}"));
    }

    public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
    {
        int FinalCleanupIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Final Cleanup"));

        if (FinalCleanupIndex != -1)
        {
            tasks.Insert(FinalCleanupIndex + 1, new FloodGenPass("Flood", 100f));
        }
    }

    public override void PostUpdateEverything()
    {
        if (Main.netMode == NetmodeID.MultiplayerClient) return;

        foreach (Player player in Main.player)
        {
            if (player.active)
            {
                Rectangle area = new Rectangle(
                    (int)(player.position.X / 16) - 100,
                    (int)(player.position.Y / 16) - 100,
                    200,
                    200
                );

                FloodArea(area);
            }
        }
    }

    private void FloodArea(Rectangle area)
    {
        area = ClampToWorld(area);

        for (int x = area.Left; x < area.Right; x++)
        {
            for (int y = area.Top; y < area.Bottom; y++)
            {
                Tile tile = Main.tile[x, y];

                if (tile == null || tile.LiquidAmount == 255) continue;

                tile.LiquidType = LiquidID.Water;
                tile.LiquidAmount = 255;

                WorldGen.SquareTileFrame(x, y, resetFrame: true);
            }
        }
    }

    private Rectangle ClampToWorld(Rectangle area)
    {
        area.X = (int)MathHelper.Clamp(area.X, 0, Main.maxTilesX);
        area.Y = (int)MathHelper.Clamp(area.Y, FloodLevel, Main.maxTilesY);
        return area;
    }

}

public class FloodGenPass : GenPass
{
    public FloodGenPass(string name, double loadWeight) : base(name, loadWeight)
    {
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

    public static void FloodWorld()
    {
        int left = 0;
        int right = Main.maxTilesX;
        int top = FloodSystem.FloodLevel;
        int bottom = Main.maxTilesY;

        for (int x = left; x < right; x++)
        {
            for (int y = top; y < bottom; y++)
            {
                Tile tile = Main.tile[x, y];

                if (tile == null || tile.LiquidAmount == 255) continue;

                tile.LiquidType = LiquidID.Water;
                tile.LiquidAmount = 255;

                WorldGen.SquareTileFrame(x, y, resetFrame: true);
            }
        }
    }

    protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
    {
        FloodWorld();
    }
}