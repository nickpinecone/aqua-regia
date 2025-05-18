using Terraria;
using Terraria.DataStructures;
using WaterGuns.Modules.Projectiles;
using WaterGuns.Sources;
using WaterGuns.Utils;

namespace WaterGuns.NPCs.Swimmer;

public class SwimmerProjectile : BaseProjectile
{
    public override string Texture => TexturesPath.Empty;

    public PropertyModule Property { get; private set; }
    public WaterModule Water { get; private set; }

    public SwimmerProjectile() : base()
    {
        Property = new PropertyModule();
        Water = new WaterModule();

        Composite.AddModule(Property, Water);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Water.SetDefaults();
        Property.SetProperties(this, 16, 16, 1, 1);
        Property.SetTimeLeft(this, 35);
        Property.SetGravity();
    }

    public override void OnSpawn(IEntitySource source)
    {
        base.OnSpawn(source);

        Projectile.position += Projectile.velocity * 1.5f;

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
