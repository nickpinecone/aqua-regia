
using Microsoft.Xna.Framework;
using Terraria;
using WaterGuns.Projectiles.Modules;

namespace WaterGuns.Projectiles.Wooden;

public class WoodenProjectile : BaseProjectile
{
    public override string Texture => "WaterGuns/Assets/Textures/Empty";

    public PropertyModule Property { get; private set; }
    public VisualModule Visual { get; private set; }

    public WoodenProjectile() : base()
    {
        Property = new PropertyModule(this);
        Visual = new VisualModule(this);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Visual.SetWater();
        Property.SetDefaults(this);
        Property.SetDefaultGravity();

        Projectile.damage = 1;
        Projectile.penetrate = 1;
        Projectile.timeLeft = 35;
        Property.DefaultTime = 35;

        Projectile.width = 16;
        Projectile.height = 16;
    }

    public override void OnHitNPC(Terraria.NPC target, Terraria.NPC.HitInfo hit, int damageDone)
    {
        base.OnHitNPC(target, hit, damageDone);

        Visual.KillEffect(Projectile.Center);

        if (Main.rand.Next(0, 6) == 0)
        {
            var position = target.Center - new Vector2(0, target.height * 1.5f);

            SpawnProjectile<AcornProjectile>(position, Vector2.Zero, hit.Damage, hit.Knockback);
        }
    }

    public override void AI()
    {
        base.AI();

        Projectile.velocity = Property.ApplyGravity(Projectile.velocity);
        Visual.CreateDust(Projectile.Center, Projectile.velocity);
    }
}
