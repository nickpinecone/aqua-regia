using AquaRegia.Library.Global;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace AquaRegia.Mobs.Jellyfish;

public class Jellyfish : ModSystem
{
    public override void Load()
    {
        NpcGlobal.ModifyNPCLootEvent += OnModifyNPCLootEvent;
    }

    private void OnModifyNPCLootEvent(NPC npc, NPCLoot npcLoot)
    {
        if (npc.type is NPCID.BlueJellyfish or NPCID.PinkJellyfish or NPCID.GreenJellyfish)
        {
            npcLoot.Add(new CommonDrop(ItemID.Gel, 2, 1, 4));
        }
    }
}