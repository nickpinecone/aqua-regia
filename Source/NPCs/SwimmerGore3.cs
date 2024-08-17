using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using AquaRegia.Utils;

namespace AquaRegia.NPCs;

public class SwimmerGore3 : ModGore
{
    public override string Texture => TexturesPath.NPCs + "SwimmerGore3";

    public override void OnSpawn(Terraria.Gore gore, IEntitySource source)
    {
        gore.timeLeft = Gore.goreTime * 3;
        base.OnSpawn(gore, source);
    }
}
