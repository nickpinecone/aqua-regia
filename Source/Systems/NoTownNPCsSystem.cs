using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AquaRegia.Systems;

public class NoTownNPCsSystem : ModSystem
{
    public override void SetStaticDefaults()
    {
        for (var i = 0; i < NPCID.Count; i++)
        {
            Main.townNPCCanSpawn[i] = false;
        }
    }
}