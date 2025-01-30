using AquaRegia.Modules.Mobs;
using AquaRegia.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace AquaRegia.World.CoralReef.Mobs;

public class Clownfish : BaseMob
{
    public override string Texture => TexturesPath.World + "CoralReef/Mobs/Clownfish";

    public SwimModule Swim { get; private set; }
    public PropertyModule Property { get; private set; }

    private int _swimDirection = -1;
    private float _swimVertical = 0.1f;
    private Vector2 _baseLevel = Vector2.Zero;

    public Clownfish()
    {
        Swim = new SwimModule();
        Property = new PropertyModule();

        Composite.AddModule(Swim, Property);
    }

    public override void SetDefaults()
    {
        Property.SetProperties(this, 24, 12, 0, 0, 5, 0.5f, true);
        Swim.SetGravity(gravityCap: 9);
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
        bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
            BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,
            new FlavorTextBestiaryInfoElement("A simple clownfish found in the Coral Reef")
        });
    }

    public override void AI()
    {
        base.AI();

        NPC.velocity = Swim.ApplyGravity(NPC.getRect(), NPC.velocity);

        if (Swim.IsOnLand(NPC.getRect()))
        {
            NPC.velocity = (-Vector2.UnitY).RotatedByRandom(MathHelper.PiOver2) * Main.rand.NextFloat(0.5f, 3f);
        }

        if (Swim.IsInWater(NPC.getRect()))
        {
            NPC.velocity.X = 2f * _swimDirection;
        }

        if (Swim.IsInWater(NPC.getRect()) && Swim.Gravity == 0)
        {
            if (_baseLevel == Vector2.Zero)
            {
                _baseLevel = NPC.Center;
            }
            NPC.velocity.Y = _swimVertical;

            if ((NPC.Center.Y > _baseLevel.Y + 3f && _swimVertical > 0) ||
                (NPC.Center.Y < _baseLevel.Y - 3f && _swimVertical < 0))
            {
                _swimVertical = -_swimVertical;
            }

            if (TileHelper.AnySolidInArea(NPC.Center + new Vector2(16 * _swimDirection, 0), 1, 2))
            {
                _swimDirection = -_swimDirection;
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
        else
        {
            if (NPC.velocity.Y < 0)
            {
                var angle = Helper.AngleBetween(-Vector2.UnitY, NPC.velocity);
                NPC.rotation = angle * 0.1f;
            }
        }
    }
}
