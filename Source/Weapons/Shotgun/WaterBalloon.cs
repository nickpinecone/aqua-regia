using Microsoft.Xna.Framework;
using Terraria;
using AquaRegia.Modules;
using AquaRegia.Modules.Projectiles;
using AquaRegia.Utils;

namespace AquaRegia.Weapons.Shotgun;

public class WaterBalloon : BaseProjectile
{
    public override string Texture => TexturesPath.Weapons + "Shotgun/WaterBalloon";

    public PropertyModule Property { get; private set; }
    public WaterModule Water { get; private set; }

    public WaterBalloon() : base()
    {
        Property = new PropertyModule(this);
        Water = new WaterModule(this);

        IsAmmoRuntime = true;
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.SetDefaults(this, 22, 22, 1, 1, 0, 0, 100);
        Property.SetTimeLeft(this, 25);
        Property.SetGravity();
    }

    public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
    {
        base.OnSpawn(source);

        Water.ApplyAmmo(_source.Ammo);
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
