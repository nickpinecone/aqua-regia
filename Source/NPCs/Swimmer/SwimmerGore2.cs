using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using AquaRegia.Utils;

namespace AquaRegia.NPCs.Swimmer;

public class SwimmerGore2 : ModGore
{
    public override string Texture => TexturesPath.NPCs + "Swimmer/SwimmerGore2";

    public override void OnSpawn(Terraria.Gore gore, IEntitySource source)
    {
        gore.timeLeft = Gore.goreTime * 3;
        base.OnSpawn(gore, source);
    }
}
