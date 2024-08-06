using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using WaterGuns.Utils;

namespace WaterGuns.NPC;

public class Swimmer_Gore3 : ModGore
{
    public override string Texture => TexturesPath.NPC + "Swimmer_Gore3";

    public override void OnSpawn(Terraria.Gore gore, IEntitySource source)
    {
        gore.timeLeft = Gore.goreTime * 3;
        base.OnSpawn(gore, source);
    }
}
