using AquaRegia.Modules;
using AquaRegia.Modules.Projectiles;
using AquaRegia.Utils;
using Microsoft.Xna.Framework;
using Terraria;

namespace AquaRegia.Weapons.Sunflower;

public class SeedProjectile : BaseProjectile
{
    public override string Texture => TexturesPath.Weapons + "Sunflower/SeedProjectile";

    public PropertyModule Property { get; private set; }

    public SeedProjectile() : base()
    {
        Property = new PropertyModule(this);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.SetDefaults(this, 8, 8, 1, 1, 1f, 0, 0);
        Property.SetTimeLeft(this, 120);
        Property.SetGravity();
    }

    public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
    {
        base.OnSpawn(source);

        var velocity = (new Vector2(0, -1)).RotatedByRandom(MathHelper.PiOver4);
        velocity.Normalize();
        velocity *= Main.rand.NextFloat(12f, 16f);

        Projectile.velocity = velocity;
    }

    public override void OnKill(int timeLeft)
    {
        base.OnKill(timeLeft);

        SpawnProjectile<SeedExplosion>(Projectile.Center, Vector2.Zero, Projectile.damage, Projectile.knockBack);
    }

    public override void AI()
    {
        base.AI();

        Projectile.velocity = Property.ApplyGravity(Projectile.velocity);
    }
}
