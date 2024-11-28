using System.Collections.Generic;
using AquaRegia.World.CoralReef;
using Terraria.ModLoader;

public class NpcGlobal : GlobalNPC
{
    public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
    {
        base.EditSpawnPool(pool, spawnInfo);

        CoralReefBiome.EditSpawnPool(pool, spawnInfo);
    }
}
