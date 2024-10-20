using AquaRegia.Utils;
using Terraria;
using Terraria.ModLoader;

namespace AquaRegia.World;

public class CoralReef : ModBiome
{
    public static int ReefLevel = 0;

    public override void OnEnter(Player player)
    {
        ChatLog.Message("Player has entered the coral reef biome!");
    }

    public override bool IsBiomeActive(Player player)
    {
        // If we are on the left ocean
        return ((player.Center.X < WorldGen.oceanDistance * 16 ||
                 // Or on the right ocean
                 player.Center.X > (Main.maxTilesX - WorldGen.oceanDistance) * 16) &&
                // And we are below ocean level
                player.Center.Y > ReefLevel * 16);
        // Then check for at lest x amount of coral tiles
    }
}
