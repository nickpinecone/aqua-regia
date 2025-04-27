using WaterGuns.Modules;
using WaterGuns.Modules.Projectiles;
using WaterGuns.Utils;
using Microsoft.Xna.Framework;
using Terraria;

namespace WaterGuns.Weapons.Sunflower;

public class SeedProjectile : BaseProjectile
{
    public override string Texture => TexturesPath.Weapons + "Sunflower/SeedProjectile";

    public PropertyModule Property { get; private set; }

    public SeedProjectile() : base()
    {
        Composite.AddRuntimeModule(ImmunityModule.New());

        Property = new PropertyModule();

        Composite.AddModule(Property);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.SetProperties(this, 8, 8, 1, 1, 1f, 0, 0);
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

        Helper.SpawnProjectile<SeedExplosion>(Projectile.GetSource_FromThis(), Owner, Projectile.Center, Vector2.Zero, Projectile.damage, Projectile.knockBack);
    }

    public override void AI()
    {
        base.AI();

        Projectile.velocity = Property.ApplyGravity(Projectile.velocity);
    }
}
