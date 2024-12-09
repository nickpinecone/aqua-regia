using Microsoft.Xna.Framework;
using Terraria;
using AquaRegia.Modules;
using AquaRegia.Modules.Projectiles;
using AquaRegia.Utils;
using Terraria.ID;

namespace AquaRegia.Weapons.Ice;

public class FrostShard : BaseProjectile
{
    public override string Texture => TexturesPath.Weapons + "Ice/FrostShard";

    public PropertyModule Property { get; private set; }

    public FrostShard() : base()
    {
        Property = new PropertyModule(this);
    }

    public override void SetStaticDefaults()
    {
        Main.projFrames[Projectile.type] = 5;
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.SetDefaults(this, 16, 16, 1, 1);
        Property.SetTimeLeft(this, 50);
    }

    public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
    {
        base.OnSpawn(source);

        Projectile.frame = Main.rand.Next(0, 5);
    }

    public override void OnKill(int timeLeft)
    {
        base.OnKill(timeLeft);

        foreach (var particle in Particle.Circle(DustID.Snow, Projectile.Center, new Vector2(4, 4), 6, 1f))
        {
            particle.noGravity = true;
        }
    }

    public override void OnHitNPC(Terraria.NPC target, Terraria.NPC.HitInfo hit, int damageDone)
    {
        base.OnHitNPC(target, hit, damageDone);

        if (Main.rand.Next(0, 8) == 0)
        {
            target.AddBuff(BuffID.Frostburn, 120);
        }
    }

    public override void AI()
    {
        base.AI();

        Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
        var particle = Particle.Single(DustID.Snow, Projectile.Center, new Vector2(1, 1), Vector2.Zero, 0.8f);
        particle.noGravity = true;
    }
}
