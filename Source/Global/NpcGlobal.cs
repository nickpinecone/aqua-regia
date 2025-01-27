using Terraria;
using Terraria.ModLoader;

namespace AquaRegia.Global;

public class NpcGlobal : GlobalNPC
{
    public delegate void ModifyNPCLootDelegate(NPC npc, NPCLoot npcloot);
    public static event ModifyNPCLootDelegate? ModifyNPCLootEvent;
    public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
    {
        ModifyNPCLootEvent?.Invoke(npc, npcLoot);
    }
}
