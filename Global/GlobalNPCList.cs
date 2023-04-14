using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;

namespace WaterGuns.Global
{
    public class GlobalNPCList : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.SantaNK1)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Hardmode.MiniWaterGun>()));
            }
            else if (npc.type == NPCID.Golem)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Hardmode.AncientWaterGun>()));
            }
            else if (npc.type == NPCID.DukeFishron)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Hardmode.OceanWaterGun>()));
            }
            else if (npc.type == NPCID.IchorSticker)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Hardmode.IchorStickerGun>(), 8));
            }
            else if (npc.type == NPCID.Deerclops)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.PreHardmode.IceWaterGun>()));
            }
            base.ModifyNPCLoot(npc, npcLoot);
        }
    }
}
