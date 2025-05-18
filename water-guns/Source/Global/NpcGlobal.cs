using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace WaterGuns.Global;

public class NpcGlobal : GlobalNPC
{
    public delegate void EditSpawnPoolDelegate(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo);
    public static event EditSpawnPoolDelegate? EditSpawnPoolEvent;
    public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
    {
        EditSpawnPoolEvent?.Invoke(pool, spawnInfo);
    }

    public delegate void ModifyNPCLootDelegate(NPC npc, NPCLoot npcLoot);
    public static event ModifyNPCLootDelegate? ModifyNPCLootEvent;
    public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
    {
        ModifyNPCLootEvent?.Invoke(npc, npcLoot);
    }
}
