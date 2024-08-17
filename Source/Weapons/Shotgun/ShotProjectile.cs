using Terraria;
using AquaRegia.Modules;
using AquaRegia.Modules.Projectiles;
using AquaRegia.Utils;

namespace AquaRegia.Weapons.Shotgun;

public class ShotProjectile : BaseProjectile
{
    public override string Texture => TexturesPath.Empty;

    public PropertyModule Property { get; private set; }
    public WaterModule Water { get; private set; }

    public ShotProjectile() : base()
    {
        Property = new PropertyModule(this);
        Water = new WaterModule(this);

        IsAmmoRuntime = true;
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Water.SetDefaults();
        Property.SetDefaults(this);
        Property.SetDefaultGravity();
        Property.SetTimeLeft(this, 20);

        Projectile.damage = 1;
        Projectile.penetrate = 1;

        Projectile.width = 16;
        Projectile.height = 16;
    }

    public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
    {
        base.OnSpawn(source);

        Water.ApplyAmmo(_source.Ammo);
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
