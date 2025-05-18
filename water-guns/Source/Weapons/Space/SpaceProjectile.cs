using Terraria;
using WaterGuns.Modules;
using WaterGuns.Modules.Projectiles;
using WaterGuns.Utils;
using Microsoft.Xna.Framework;
using WaterGuns.Sources;

namespace WaterGuns.Weapons.Space;

public class SpaceProjectile : BaseProjectile
{
    public override string Texture => TexturesPath.Empty;

    public PropertyModule Property { get; private set; }
    public WaterModule Water { get; private set; }
    public BounceModule Bounce { get; private set; }

    public SpaceProjectile() : base()
    {
        Composite.AddRuntimeModule(ImmunityModule.New());

        Property = new PropertyModule();
        Water = new WaterModule();
        Bounce = new BounceModule();

        Composite.AddModule(Property, Water, Bounce);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Bounce.SetDefaults(2);
        Water.SetDefaults();

        Property.SetProperties(this, 16, 16, 1, 2);
        Property.SetTimeLeft(this, 35);
        Property.SetGravity();
    }

    public override bool OnTileCollide(Microsoft.Xna.Framework.Vector2 oldVelocity)
    {
        base.OnTileCollide(oldVelocity);

        var result = Bounce.Calculate(oldVelocity, Projectile.velocity);

        if (result == null)
        {
            return true;
        }
        else
        {
            Projectile.velocity = (Vector2)result;
            Projectile.timeLeft = Property.DefaultTime;
            Property.Gravity = Property.DefaultGravity;
            return false;
        }
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

    public override void OnHitNPC(Terraria.NPC target, Terraria.NPC.HitInfo hit, int damageDone)
    {
        base.OnHitNPC(target, hit, damageDone);
    }

    public override void AI()
    {
        base.AI();

        Projectile.velocity = Property.ApplyGravity(Projectile.velocity);
        Water.CreateDust(Projectile.Center, Projectile.velocity);
    }
}
