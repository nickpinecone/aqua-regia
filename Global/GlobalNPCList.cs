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
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Hardmode.SeaWaterGun>()));
            }
            else if (npc.type == NPCID.IchorSticker)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Hardmode.IchorStickerGun>(), 8));
            }
            base.ModifyNPCLoot(npc, npcLoot);
        }

        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            // Make merchant sell bottled water
            if (type == NPCID.Merchant)
            {
                shop.item[nextSlot].SetDefaults(ItemID.BottledWater);
                nextSlot++;
            }
            else if (type == NPCID.PartyGirl)
            {
                if (Main.hardMode)
                {
                    shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Hardmode.WaterBallonGun>());
                    nextSlot++;
                }
            }
            base.SetupShop(type, shop, ref nextSlot);
        }
    }
}
