using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.NPCs
{
    [AutoloadHead]
    public class Swimmer : ModNPC
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            Main.npcFrameCount[NPC.type] = 23;
            NPCID.Sets.AttackFrameCount[NPC.type] = NPCID.Sets.AttackType[NPCID.Dryad];
            NPCID.Sets.ExtraFramesCount[NPC.type] = 0;
            NPCID.Sets.DangerDetectRange[NPC.type] = 250;
            NPCID.Sets.AttackType[NPC.type] = NPCID.Sets.AttackType[NPCID.ArmsDealer];
            NPCID.Sets.AttackTime[NPC.type] = 30;
            NPCID.Sets.HatOffsetY[NPC.type] = 4;

            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Velocity = 1f,
                Direction = 1
            };

            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

            NPC.Happiness
                .SetBiomeAffection<OceanBiome>(AffectionLevel.Like)
                .SetBiomeAffection<DesertBiome>(AffectionLevel.Dislike)
                .SetNPCAffection(NPCID.Stylist, AffectionLevel.Love)
                .SetNPCAffection(NPCID.Pirate, AffectionLevel.Love)
                .SetNPCAffection(NPCID.Angler, AffectionLevel.Like)
                .SetNPCAffection(NPCID.Princess, AffectionLevel.Like)
                .SetNPCAffection(NPCID.GoblinTinkerer, AffectionLevel.Dislike)
                .SetNPCAffection(NPCID.DD2Bartender, AffectionLevel.Dislike)
            ;
        }

        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.width = 28;
            NPC.height = 52;
            NPC.aiStyle = 7;
            NPC.defense = 5;
            NPC.lifeMax = 250;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.1f;
            AnimationType = NPCID.Dryad;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean
            });
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, ModContent.GoreType<Gores.Swimmer_Gore1>());
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, ModContent.GoreType<Gores.Swimmer_Gore2>());
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, ModContent.GoreType<Gores.Swimmer_Gore3>());
            }
            base.HitEffect(hitDirection, damage);
        }

        public override List<string> SetNPCNameList()
        {
            return new List<string>()
            {
                "Lana",
                "Marina",
                "Anna",
                "Minami",
                "Morgana",
                "Mary",
                "Aqua",
                "Ridley",
                "Sonia",
                "Nami",
                "Joyce"
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

            if (Main.hardMode)
            {
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Hardmode.WaterBallonGun>());
                nextSlot++;
            }

            if (NPC.downedQueenBee)
            {
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.PreHardmode.ChainedWaterGun>());
                nextSlot++;
            }
        }

        public override string GetChat()
        {
            NPC.FindFirstNPC(ModContent.NPCType<Swimmer>());
            switch (Main.rand.Next(4))
            {
                case 0: return "Hello! It's lovely weather today, isn't it?";
                case 1: return "I think sunny days are the best.";
                case 2: return "I love going to the beach!";
                case 3: return "Did you know Water Guns use Bottled Water as ammo?";
                default: return "DefaultText";
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
            closeness = 12;
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
