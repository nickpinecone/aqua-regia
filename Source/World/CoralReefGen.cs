using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Input;
using Terraria.ID;
using Microsoft.Xna.Framework;
using AquaRegia.Utils;
using Terraria.GameContent.Personalities;
using System.Collections.Generic;

namespace AquaRegia.World;

public class CoralReefGen : ModSystem
{
    public static bool JustPressed(Keys key)
    {
        return Main.keyState.IsKeyDown(key) && !Main.oldKeyState.IsKeyDown(key);
    }

    public override void PostUpdateWorld()
    {
        if (JustPressed(Keys.D9))
        {
            DebugInfo();
        }

        if (JustPressed(Keys.D0))
        {
            TestMethod((int)Main.MouseWorld.X / 16, (int)Main.MouseWorld.Y / 16);
        }
    }

    private void DebugInfo()
    {
        ChatLog.Message(Main.LocalPlayer.Center.ToTileCoordinates().ToVector2(), "Player Center: ");
        ChatLog.Message(WorldGen.oceanDistance.ToString(), "Ocean Distance: ");
        ChatLog.Message(Main.maxTilesX.ToString(), "Max Tiles X: ");
        ChatLog.Message((Main.maxTilesX - WorldGen.oceanDistance).ToString(), "Max Tiles X - Ocean Distance: ");
        ChatLog.Message(WorldGen.oceanLevel.ToString(), "Ocean Level: ");
        var ocean = new OceanBiome();
        ChatLog.Message(ocean.IsInBiome(Main.LocalPlayer).ToString(), "Is In Ocean: ");
    }

    private void TestMethod(int x, int y)
    {
        // t_* means tile coordinates, w_* means world cooridantes
        // Dig a square in the middle of the ocean down, with curved edges
        const int depth = 200;
        const int curve_amount = 50;

        var t_edge_tiles = new List<Point>();

        var reefWidth = WorldGen.oceanDistance / 1.2f;
        var t_oceanX = Main.maxTilesX - reefWidth;
        var w_scanFrom = new Vector2(t_oceanX * 16, 0);
        var t_tilePos = TileHelper.FirstSolidFromTop(w_scanFrom, 512 * 16) ?? Point.Zero;

        var round = new Animation<int>((int)reefWidth / 2, Ease.Out);
        var back_round = new Animation<int>((int)reefWidth / 2, Ease.In, [round]);

        for (int i = (int)t_oceanX; i < Main.maxTilesX; i++)
        {
            var round_rate = round.Animate(0, curve_amount) ?? curve_amount;
            var back_rate = back_round.Animate(0, curve_amount) ?? 0;
            int last_j = 0;

            for (int j = t_tilePos.Y; j < t_tilePos.Y + (depth - curve_amount) + round_rate - back_rate; j++)
            {
                last_j = j;

                if (i == (int)t_oceanX)
                {
                    t_edge_tiles.Add(new Point(i, j));
                }

                WorldGen.KillTile(i, j, noItem: true);
                Tile tile = Main.tile[i, j];
                tile.LiquidAmount = 255;
                tile.LiquidType = LiquidID.Water;
            }

            t_edge_tiles.Add(new Point(i, last_j));
        }

        // Place "coral" splotches along the edges
        foreach (var t_tile in t_edge_tiles)
        {
            WorldGen.TileRunner(t_tile.X, t_tile.Y, WorldGen.genRand.Next(3, 8), WorldGen.genRand.Next(2, 8),
                                TileID.Adamantite, overRide: true, addTile: true);
        }
    }
}
