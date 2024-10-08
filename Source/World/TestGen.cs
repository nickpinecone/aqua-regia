using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Input;
using Terraria.ID;
using Microsoft.Xna.Framework;
using AquaRegia.Utils;
using Terraria.GameContent.Personalities;

namespace AquaRegia.World;

public class TestGen : ModSystem
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
        ChatLog.Message(WorldGen.oceanDistance.ToString());
        ChatLog.Message(Main.maxTilesX.ToString());
        ChatLog.Message((Main.maxTilesX - WorldGen.oceanDistance).ToString());
        var ocean = new OceanBiome();
        ChatLog.Message(ocean.IsInBiome(Main.LocalPlayer).ToString());

        ChatLog.Message(WorldGen.oceanLevel.ToString());
    }

    private void TestMethod(int x, int y)
    {
        Dust.QuickBox(new Vector2(x, y) * 16, new Vector2(x + 1, y + 1) * 16, 2, Color.YellowGreen, null);

        // Only approximately, near the place where water starts
        // var oceanStartX = Main.maxTilesX - WorldGen.oceanDistance;

        // Again, also very approximate
        var oceanCenterX = Main.maxTilesX - WorldGen.oceanDistance / 3f;
        // var oceanCenterX = x;

        // TODO actually calculate the ocean center, cause above could be a sky island?
        // var oceanCenterY = 0;
        var oceanCenterY = WorldGen.oceanLevel - 200;

        // Dig tunnel is inconsistent and makes for weird generations
        // Unless i figure this out, probably have to make an hand-made algorithm using WorldGen.KillTile
        WorldGen.digTunnel(oceanCenterX, oceanCenterY, 0.5, 1, 1000, (int)(WorldGen.oceanDistance / 2f));

        // WorldGen.TileRunner(x - 1, y, WorldGen.genRand.Next(3, 8), WorldGen.genRand.Next(2, 8), TileID.CobaltBrick);
    }
}
