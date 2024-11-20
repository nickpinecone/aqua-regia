using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AquaRegia.World.CoralReef;

public class CoralReefBiome : ModBiome
{
    public override int Music => MusicID.Ocean;

    public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;

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
