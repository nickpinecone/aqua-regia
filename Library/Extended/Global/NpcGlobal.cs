using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace AquaRegia.Library.Extended.Global;

public class NpcGlobal : GlobalNPC
{
    public delegate void ModifyNPCLootDelegate(NPC npc, NPCLoot npcLoot);
    public static event ModifyNPCLootDelegate? ModifyNPCLootEvent;
    public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
    {
        ModifyNPCLootEvent?.Invoke(npc, npcLoot);
    }

    public delegate void EditSpawnPoolDelegate(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo);
    public static event EditSpawnPoolDelegate? EditSpawnPoolEvent;
    public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
    {
        pool.Clear();

        EditSpawnPoolEvent?.Invoke(pool, spawnInfo);
    }

    public override void Unload()
    {
        ModifyNPCLootEvent = null;
        EditSpawnPoolEvent = null;
    }
}