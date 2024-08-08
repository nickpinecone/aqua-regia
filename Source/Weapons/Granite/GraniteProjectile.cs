using Microsoft.Xna.Framework;
using Terraria;
using WaterGuns.Modules;
using WaterGuns.Modules.Projectiles;
using WaterGuns.Utils;

namespace WaterGuns.Weapons.Granite;

public class GraniteProjecitle : BaseProjectile
{
    public override string Texture => TexturesPath.Empty;

    public PropertyModule Property { get; private set; }
    public WaterModule Water { get; private set; }

    public GraniteProjecitle() : base()
    {
        Property = new PropertyModule(this);
        Water = new WaterModule(this);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Water.SetDefaults();
        Property.SetDefaults(this);
        Property.SetDefaultGravity();
        Property.SetTimeLeft(this, 35);

        Projectile.damage = 1;
        Projectile.penetrate = 1;

        Projectile.width = 16;
        Projectile.height = 16;
    }

    public override void OnKill(int timeLeft)
    {
        base.OnKill(timeLeft);

        Water.KillEffect(Projectile.Center, Projectile.velocity);
    }

    public override void OnHitNPC(Terraria.NPC target, Terraria.NPC.HitInfo hit, int damageDone)
    {
        base.OnHitNPC(target, hit, damageDone);

        // if (Main.rand.Next(0, 3) == 0)
        {
            var source = new GraniteSource(Projectile.GetSource_NaturalSpawn());
            source.Target = target;

            SpawnProjectile<GraniteChunk>(target.Center, Vector2.Zero, hit.Damage, hit.Knockback * 4, source);
        }
    }

    public override void AI()
    {
        base.AI();

        Projectile.velocity = Property.ApplyGravity(Projectile.velocity);
        Water.CreateDust(Projectile.Center, Projectile.velocity);
    }
}
