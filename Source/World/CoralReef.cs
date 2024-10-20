using AquaRegia.Utils;
using Terraria;
using Terraria.ModLoader;

namespace AquaRegia.World;

public class CoralReef : ModBiome
{
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
                // Then check for at least x amount of coral tiles
                ModContent.GetInstance<CoralReefGen>().CoralTileCount >= 40);
    }
}
