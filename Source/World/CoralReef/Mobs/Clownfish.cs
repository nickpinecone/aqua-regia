using AquaRegia.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace AquaRegia.World.CoralReef.Mobs;

public class Clownfish : ModNPC
{
    public override string Texture => TexturesPath.World + "CoralReef/Mobs/Clownfish";

    public override void SetDefaults()
    {
        NPC.noGravity = true;
        NPC.width = 32;
        NPC.height = 16;
        NPC.damage = 0;
        NPC.defense = 0;
        NPC.lifeMax = 5;
        NPC.HitSound = SoundID.NPCHit1;
        NPC.DeathSound = SoundID.NPCDeath1;
        NPC.knockBackResist = 0.5f;
        NPC.aiStyle = 16;

        NPC.HitSound = SoundID.NPCHit1;
        NPC.DeathSound = SoundID.NPCDeath1;

        AIType = NPCID.Goldfish;
    }

    public override void OnKill()
    {
        base.OnKill();

        foreach (var particle in Particle.Circle(DustID.RedStarfish, NPC.Center, new Vector2(4, 4), 4, 1f))
        {
            particle.noGravity = true;
        }
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
        if (spawnInfo.Player.InModBiome(ModContent.GetInstance<CoralReefBiome>()))
        {
            return 0.2f;
        }

        return 0f;
    }

    public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
    {
        bestiaryEntry.Info.AddRange(
            new IBestiaryInfoElement[] {// Sets the spawning conditions of this NPC that is listed in the bestiary.
                                        BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,

                                        // Sets the description of this NPC that is listed in the bestiary.
                                        new FlavorTextBestiaryInfoElement("Found in the coral reef biome")
            });
    }

    public override void AI()
    {
        base.AI();

        if (NPC.velocity.X > 0)
        {
            NPC.rotation = NPC.velocity.ToRotation();
            NPC.spriteDirection = 1;
        }
        else
        {
            NPC.rotation = NPC.velocity.ToRotation() - MathHelper.Pi;
            NPC.spriteDirection = -1;
        }
    }
}
