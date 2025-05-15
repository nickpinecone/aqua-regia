using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace WaterGuns.Library.Global;

public class NpcGlobal : GlobalNPC
{
    public delegate void ModifyNPCLootDelegate(NPC npc, NPCLoot npcLoot);
    public static event ModifyNPCLootDelegate? ModifyNPCLootEvent;
    public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
    {
        ModifyNPCLootEvent?.Invoke(npc, npcLoot);
    }
}
