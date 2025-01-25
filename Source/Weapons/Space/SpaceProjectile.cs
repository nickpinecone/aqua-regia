using Terraria;
using AquaRegia.Modules;
using AquaRegia.Modules.Projectiles;
using AquaRegia.Utils;
using Microsoft.Xna.Framework;

namespace AquaRegia.Weapons.Space;

public class SpaceProjectile : BaseProjectile
{
    public override string Texture => TexturesPath.Empty;

    public PropertyModule Property { get; private set; }
    public WaterModule Water { get; private set; }
    public BounceModule Bounce { get; private set; }

    public SpaceProjectile() : base()
    {
        Property = new PropertyModule(this);
        Water = new WaterModule(this);
        Bounce = new BounceModule(this);

        IsAmmoRuntime = true;
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Bounce.SetDefaults(Property, 2);
        Water.SetDefaults();

        Property.SetDefaults(this, 16, 16, 1, 2);
        Property.SetTimeLeft(this, 35);
        Property.SetGravity();
    }

    public override bool OnTileCollide(Microsoft.Xna.Framework.Vector2 oldVelocity)
    {
        base.OnTileCollide(oldVelocity);

        var result = Bounce.Update(this, oldVelocity, Projectile.velocity);

        if (result == null)
        {
            return true;
        }
        else
        {
            Projectile.velocity = (Vector2)result;
            return false;
        }
    }

    public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
    {
        base.OnSpawn(source);

        Water.ApplyAmmo(_source?.Ammo);
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
