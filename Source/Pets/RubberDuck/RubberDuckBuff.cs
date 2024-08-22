using AquaRegia.Utils;
using Terraria;
using Terraria.ModLoader;

namespace AquaRegia.Pets.RubberDuck;

public class RubberDuckBuff : ModBuff
{
    public override string Texture => TexturesPath.Pets + "RubberDuck/RubberDuckBuff";

    public override void SetStaticDefaults()
    {
        Main.buffNoTimeDisplay[Type] = true;
        Main.vanityPet[Type] = true;
    }

    public override void Update(Player player, ref int buffIndex)
    {
        bool unused = false;
        player.BuffHandle_SpawnPetIfNeededAndSetTime(buffIndex, ref unused,
                                                     ModContent.ProjectileType<RubberDuckProjectile>());
    }
}
