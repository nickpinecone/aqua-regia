using System;
using AquaRegia.Modules.Mobs;
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

    public SwimModule Swim { get; set; }

    public Clownfish()
    {
        Swim = new SwimModule();
    }

    public override void SetDefaults()
    {
        Swim.SetGravity(gravityCap: 9);

        NPC.noGravity = true;
        NPC.width = 32;
        NPC.height = 16;
        NPC.damage = 0;
        NPC.defense = 0;
        NPC.lifeMax = 5;
        NPC.HitSound = SoundID.NPCHit1;
        NPC.DeathSound = SoundID.NPCDeath1;
        NPC.knockBackResist = 0.5f;

        NPC.HitSound = SoundID.NPCHit1;
        NPC.DeathSound = SoundID.NPCDeath1;
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

    private bool IsTileCollide()
    {
        return Collision.SolidTiles(NPC.position - new Vector2(4, 4), NPC.width + 8, NPC.height + 8);
    }

    private int direction = -1;
    private float vertical = 0.1f;
    private Vector2 level = Vector2.Zero;
    public override void AI()
    {
        base.AI();

        NPC.velocity = Swim.ApplyGravity(NPC.Center, NPC.velocity);

        if (Swim.IsInWater(NPC.Center))
        {
            NPC.velocity.X = 2f * direction;
        }

        if (Swim.IsInWater(NPC.Center) && Swim.Gravity == 0)
        {
            if (level == Vector2.Zero)
            {
                level = NPC.Center;
            }
            NPC.velocity.Y = vertical;

            if ((NPC.Center.Y > level.Y + 3f && vertical > 0) || (NPC.Center.Y < level.Y - 3f && vertical < 0))
            {
                vertical = -vertical;
            }

            if (TileHelper.AnySolidInArea(NPC.Center + new Vector2(16 * direction, 0), 1, 2))
            {
                direction = -direction;
            }

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
}
