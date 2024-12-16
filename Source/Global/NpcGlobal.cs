using System.Collections.Generic;
using AquaRegia.Weapons.Ice;
using AquaRegia.Weapons.Sunflower;
using AquaRegia.World.CoralReef;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace AquaRegia.Global;

public class NpcGlobal : GlobalNPC
{
    public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
    {
        base.EditSpawnPool(pool, spawnInfo);

        CoralReefBiome.EditSpawnPool(pool, spawnInfo);
    }

    public override void ModifyNPCLoot(Terraria.NPC npc, NPCLoot npcLoot)
    {
        base.ModifyNPCLoot(npc, npcLoot);

        if (npc.type == NPCID.BloodZombie || npc.type == NPCID.Drippler)
        {
            npcLoot.Add(ItemDropRule.ByCondition(new DownedEvilCondition(), ModContent.ItemType<SunflowerGun>(), 50));
        }

        if (npc.type == NPCID.Deerclops)
        {
            npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ModContent.ItemType<IceGun>(), 2));
        }
    }
}
