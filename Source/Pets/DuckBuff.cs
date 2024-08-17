using AquaRegia.Utils;
using Terraria;
using Terraria.ModLoader;

namespace AquaRegia.Pets;

public class DuckBuff : ModBuff
{
    public override string Texture => TexturesPath.Pets + "DuckBuff";

    public override void SetStaticDefaults()
    {
        Main.buffNoTimeDisplay[Type] = true;
        Main.vanityPet[Type] = true;
    }

    public override void Update(Player player, ref int buffIndex)
    {
        bool unused = false;
        player.BuffHandle_SpawnPetIfNeededAndSetTime(buffIndex, ref unused,
                                                     ModContent.ProjectileType<DuckProjectile>());
    }
}
