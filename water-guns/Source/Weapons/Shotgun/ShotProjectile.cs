using Terraria;
using WaterGuns.Modules;
using WaterGuns.Modules.Projectiles;
using WaterGuns.Utils;
using WaterGuns.Sources;

namespace WaterGuns.Weapons.Shotgun;

public class ShotProjectile : BaseProjectile
{
    public override string Texture => TexturesPath.Empty;

    public PropertyModule Property { get; private set; }
    public WaterModule Water { get; private set; }

    public ShotProjectile() : base()
    {
        Composite.AddRuntimeModule(ImmunityModule.New());

        Property = new PropertyModule();
        Water = new WaterModule();

        Composite.AddModule(Property, Water);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Water.SetDefaults();
        Property.SetProperties(this, 16, 16, 1, 1);
        Property.SetTimeLeft(this, 20);
        Property.SetGravity();
    }

    public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
    {
        base.OnSpawn(source);

        if (source is WeaponWithAmmoSource custom)
        {
            custom.Ammo?.ApplyToProjectile(this);
        }
    }

    public override void OnKill(int timeLeft)
    {
        base.OnKill(timeLeft);

        Water.KillEffect(Projectile.Center, Projectile.velocity);
    }

    public override void AI()
    {
        base.AI();

        Projectile.velocity = Property.ApplyGravity(Projectile.velocity);
        Water.CreateDust(Projectile.Center, Projectile.velocity);
    }
}
