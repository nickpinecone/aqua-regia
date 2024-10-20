using AquaRegia.Modules.Mobs;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace AquaRegia.World.CoralReef.Mobs;

// These three class showcase usage of the WormHead, WormBody and WormTail classes from Worm.cs
internal class OarfishHead : BaseWormHead
{
    public override string Texture => "AquaRegia/Source/World/CoralReef/Mobs/OarfishHead";

    public override int BodyType => ModContent.NPCType<OarfishBody>();

    public override int TailType => ModContent.NPCType<OarfishTail>();

    public override void SetStaticDefaults()
    {
        // var drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers() {
        //     // Influences how the NPC looks in the Bestiary
        //     CustomTexturePath =
        //         "ExampleMod/Content/NPCs/ExampleWorm_Bestiary", // If the NPC is multiple parts like a worm, a
        //                                                         // custom texture for the Bestiary is encouraged.
        //     Position = new Vector2(40f, 24f), PortraitPositionXOverride = 0f, PortraitPositionYOverride = 12f
        // };
        // NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifier);
    }

    public override void SetDefaults()
    {
        // Head is 10 defense, body 20, tail 30.
        NPC.CloneDefaults(NPCID.DiggerHead);
        NPC.aiStyle = -1;
        NPC.noTileCollide = false;
    }

    public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
    {
        // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
        bestiaryEntry.Info.AddRange(
            new IBestiaryInfoElement[] {// Sets the spawning conditions of this NPC that is listed in the bestiary.
                                        BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,

                                        // Sets the description of this NPC that is listed in the bestiary.
                                        new FlavorTextBestiaryInfoElement("Found in the coral reef biome")
            });
    }

    public override void Init()
    {
        // Set the segment variance
        // If you want the segment length to be constant, set these two properties to the same value
        MinSegmentLength = 6;
        MaxSegmentLength = 12;

        CommonWormInit(this);
    }

    // This method is invoked from ExampleWormHead, ExampleWormBody and ExampleWormTail
    internal static void CommonWormInit(BaseWorm worm)
    {
        // These two properties handle the movement of the worm
        worm.MoveSpeed = 3.5f;
        worm.Acceleration = 0.045f;
    }

    private int attackCounter;
    public override void SendExtraAI(BinaryWriter writer)
    {
        writer.Write(attackCounter);
    }

    public override void ReceiveExtraAI(BinaryReader reader)
    {
        attackCounter = reader.ReadInt32();
    }

    public override void AI()
    {
        if (Main.netMode != NetmodeID.MultiplayerClient)
        {
            if (attackCounter > 0)
            {
                attackCounter--; // tick down the attack counter.
            }

            Player target = Main.player[NPC.target];
            // If the attack counter is 0, this NPC is less than 12.5 tiles away from its target, and has a path to
            // the target unobstructed by blocks, summon a projectile.
            if (attackCounter <= 0 && Vector2.Distance(NPC.Center, target.Center) < 200 &&
                Collision.CanHit(NPC.Center, 1, 1, target.Center, 1, 1))
            {
                Vector2 direction = (target.Center - NPC.Center).SafeNormalize(Vector2.UnitX);
                direction = direction.RotatedByRandom(MathHelper.ToRadians(10));

                // int projectile = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, direction * 1,
                //                                           ProjectileID.ShadowBeamHostile, 5, 0, Main.myPlayer);
                // Main.projectile[projectile].timeLeft = 300;
                attackCounter = 500;
                NPC.netUpdate = true;
            }
        }
    }
}

internal class OarfishBody : BaseWormBody
{
    public override string Texture => "AquaRegia/Source/World/CoralReef/Mobs/OarfishBody";

    public override void SetStaticDefaults()
    {
        NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers() {
            Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
        };
        NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
    }

    public override void SetDefaults()
    {
        NPC.CloneDefaults(NPCID.DiggerBody);
        NPC.aiStyle = -1;
    }

    public override void Init()
    {
        OarfishHead.CommonWormInit(this);
    }
}

internal class OarfishTail : BaseWormTail
{
    public override string Texture => "AquaRegia/Source/World/CoralReef/Mobs/OarfishTail";

    public override void SetStaticDefaults()
    {
        NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers() {
            Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
        };
        NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
    }

    public override void SetDefaults()
    {
        NPC.CloneDefaults(NPCID.DiggerTail);
        NPC.aiStyle = -1;
    }

    public override void Init()
    {
        OarfishHead.CommonWormInit(this);
    }
}
