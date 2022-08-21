using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.NPCs
{
    [AutoloadHead]
    public class Swimmer : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Swimmer");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.width = 32;
            NPC.height = 56;
            NPC.aiStyle = 7;
            NPC.defense = 5;
            NPC.lifeMax = 250;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.1f;
            Main.npcFrameCount[NPC.type] = 23;
            NPCID.Sets.AttackFrameCount[NPC.type] = NPCID.Sets.AttackType[NPCID.Dryad];
            NPCID.Sets.ExtraFramesCount[NPC.type] = 0;
            NPCID.Sets.DangerDetectRange[NPC.type] = 250;
            NPCID.Sets.AttackType[NPC.type] = NPCID.Sets.AttackType[NPCID.ArmsDealer];
            NPCID.Sets.AttackTime[NPC.type] = 30;
            // NPCID.Sets.AttackAverageChance[NPC.type] = 10;
            NPCID.Sets.HatOffsetY[NPC.type] = 4;
            AnimationType = NPCID.Dryad;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, ModContent.GoreType<Gores.Swimmer_Gore1>());
            base.HitEffect(hitDirection, damage);
        }

        public override bool CanTownNPCSpawn(int numTownNPCs, int money)
        {
            for (var i = 0; i < 255; i++)
            {
                Player player = Main.player[i];
                foreach (Item item in player.inventory)
                {
                    if (item.type == ItemID.BottledWater)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override List<string> SetNPCNameList()
        {
            return new List<string>()
            {
                "Lana",
                "Marina",
                "Anna"
            };
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = "Shop";
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (firstButton)
            {
                shop = true;
            }
            base.OnChatButtonClicked(firstButton, ref shop);
        }

        public override void SetupShop(Chest shop, ref int nextSlot)
        {
            shop.item[nextSlot].SetDefaults(ItemID.BottledWater, false);
            nextSlot++;
        }

        public override string GetChat()
        {
            NPC.FindFirstNPC(ModContent.NPCType<Swimmer>());
            switch (Main.rand.Next(3))
            {
                case 0: return "Hello";
                case 1: return "Test Text";
                case 2: return "I love beaches";
                default: return "Default text";
            }
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 6;
            knockback = 2f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 10;
            randExtraCooldown = 30;
        }

        public override void DrawTownAttackGun(ref float scale, ref int item, ref int closeness)
        {
            scale = 0.9f;
            item = ModContent.ItemType<Swimmer_Gun>();
            closeness = 14;
            base.DrawTownAttackGun(ref scale, ref item, ref closeness);
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ModContent.ProjectileType<Swimmer_Projectile>();
            attackDelay = 1;
        }


        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 10f;
        }
    }
}
