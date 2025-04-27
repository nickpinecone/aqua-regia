using Microsoft.Xna.Framework;
using Terraria;
using WaterGuns.Modules;
using WaterGuns.Modules.Projectiles;
using WaterGuns.Utils;
using WaterGuns.Sources;

namespace WaterGuns.Weapons.Shotgun;

public class WaterBalloon : BaseProjectile
{
    public override string Texture => TexturesPath.Weapons + "Shotgun/WaterBalloon";

    public PropertyModule Property { get; private set; }
    public WaterModule Water { get; private set; }

    public WaterBalloon() : base()
    {
        Composite.AddRuntimeModule(ImmunityModule.New());

        Property = new PropertyModule();
        Water = new WaterModule();
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.SetProperties(this, 22, 22, 1, 1, 0, 0, 100);
        Property.SetTimeLeft(this, 25);
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

        Water.KillEffect(Projectile.Center, Vector2.Zero);
    }

    public override void AI()
    {
        base.AI();

        Projectile.velocity = Property.ApplyGravity(Projectile.velocity);
    }
}
