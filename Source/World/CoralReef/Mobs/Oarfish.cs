using AquaRegia.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace AquaRegia.World.CoralReef.Mobs;

internal class OarfishHead : BaseSwimWormHead
{
    public override string Texture => TexturesPath.World + "CoralReef/Mobs/OarfishHead";

    public override int BodyType => ModContent.NPCType<OarfishBody>();
    public override int TailType => ModContent.NPCType<OarfishTail>();

    public override void SetDefaults()
    {
        NPC.defense = 10;
        NPC.lifeMax = 40;
        NPC.damage = 20;
        NPC.width = 16;
        NPC.height = 16;
        NPC.waterMovementSpeed = 1f;
        NPC.noGravity = true;
        NPC.value = 150f;

        NPC.HitSound = SoundID.NPCHit1;
        NPC.DeathSound = SoundID.NPCDeath1;
    }

    public override void OnKill()
    {
        base.OnKill();

        foreach (var particle in Particle.Circle(DustID.Water, NPC.Center, new Vector2(6, 6), 4, 1f))
        {
            particle.noGravity = true;
        }
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
        if (spawnInfo.Player.InModBiome(ModContent.GetInstance<CoralReefBiome>()))
        {
            return 0.1f;
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

    public override void Init()
    {
        MinSegmentLength = 12;
        MaxSegmentLength = 20;

        CommonWormInit(this);
    }

    internal static void CommonWormInit(BaseSwimWorm worm)
    {
        worm.MoveSpeed = 4.5f;
        worm.Acceleration = 0.045f;
    }
}

internal class OarfishBody : BaseSwimWormBody
{
    public override string Texture => TexturesPath.World + "CoralReef/Mobs/OarfishBody";

    public override void SetStaticDefaults()
    {
        NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers() {
            Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
        };
        NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
    }

    public override void SetDefaults()
    {
        NPC.defense = 20;
        NPC.lifeMax = 40;
        NPC.damage = 20;
        NPC.width = 16;
        NPC.height = 16;
        NPC.noTileCollide = true;
        NPC.noGravity = true;
        NPC.waterMovementSpeed = 1f;

        NPC.HitSound = SoundID.NPCHit1;
    }

    public override void OnKill()
    {
        base.OnKill();

        foreach (var particle in Particle.Circle(DustID.Water, NPC.Center, new Vector2(6, 6), 4, 1f))
        {
            particle.noGravity = true;
        }
    }

    public override void Init()
    {
        OarfishHead.CommonWormInit(this);
    }
}

internal class OarfishTail : BaseSwimWormTail
{
    public override string Texture => TexturesPath.World + "CoralReef/Mobs/OarfishTail";

    public override void SetStaticDefaults()
    {
        NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers() {
            Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
        };
        NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
    }

    public override void SetDefaults()
    {
        NPC.defense = 30;
        NPC.lifeMax = 40;
        NPC.damage = 20;
        NPC.width = 16;
        NPC.height = 16;
        NPC.noTileCollide = true;
        NPC.noGravity = true;
        NPC.waterMovementSpeed = 1f;

        NPC.HitSound = SoundID.NPCHit1;
    }

    public override void OnKill()
    {
        base.OnKill();

        foreach (var particle in Particle.Circle(DustID.Water, NPC.Center, new Vector2(6, 6), 4, 1f))
        {
            particle.noGravity = true;
        }
    }

    public override void Init()
    {
        OarfishHead.CommonWormInit(this);
    }
}
