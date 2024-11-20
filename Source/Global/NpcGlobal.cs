using System.Collections.Generic;
using AquaRegia.World.CoralReef;
using AquaRegia.World.CoralReef.Mobs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

public class NpcGlobal : GlobalNPC
{
    public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
    {
        if (spawnInfo.Player.InModBiome(ModContent.GetInstance<CoralReefBiome>()))
        {
            pool.Clear();

            pool.Add(ModContent.NPCType<OarfishHead>(), 1);
            pool.Add(ModContent.NPCType<Clownfish>(), 1);

            pool.Add(NPCID.Goldfish, 1);
        }
    }
}
