using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Input;
using Terraria.ID;
using Microsoft.Xna.Framework;
using AquaRegia.Utils;
using Terraria.GameContent.Personalities;

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
        // Dig a square in the middle of the ocean down
        const int depth = 100;

        var t_oceanX = Main.maxTilesX - WorldGen.oceanDistance / 1.2f;
        var w_scanFrom = new Vector2(t_oceanX * 16, 0);
        var t_tilePos = TileHelper.FirstSolidFromTop(w_scanFrom, 512 * 16) ?? Point.Zero;

        for (int i = (int)t_oceanX; i < Main.maxTilesX; i++)
        {
            for (int j = t_tilePos.Y; j < t_tilePos.Y + depth; j++)
            {
                WorldGen.KillTile(i, j, noItem: true);
                Tile tile = Main.tile[i, j];
                tile.LiquidAmount = 255;
                tile.LiquidType = LiquidID.Water;
            }
        }

        // Place "coral" splotches along the edges
        for (int j = t_tilePos.Y; j < t_tilePos.Y + depth; j++)
        {
            WorldGen.TileRunner((int)t_oceanX, j, WorldGen.genRand.Next(3, 8), WorldGen.genRand.Next(2, 8),
                                TileID.Adamantite, overRide: true, addTile: true);
        }

        for (int i = (int)t_oceanX; i < Main.maxTilesX; i++)
        {
            WorldGen.TileRunner(i, t_tilePos.Y + depth, WorldGen.genRand.Next(3, 8), WorldGen.genRand.Next(2, 8),
                                TileID.Adamantite, overRide: true, addTile: true);
        }
    }
}
