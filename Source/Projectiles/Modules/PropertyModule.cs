using Microsoft.Xna.Framework;

namespace WaterGuns.Projectiles.Modules;

public class PropertyModule : BaseProjectileModule
{
    public float Gravity { get; set; }
    public float GravityChange { get; set; }

    public PropertyModule(BaseProjectile baseProjectile) : base(baseProjectile)
    {
    }

    public void SetDefaults()
    {
        _baseProjectile.Projectile.hostile = false;
        _baseProjectile.Projectile.friendly = true;

        _baseProjectile.Projectile.damage = 1;
        _baseProjectile.Projectile.penetrate = 1;
        _baseProjectile.Projectile.timeLeft = 60;

        _baseProjectile.Projectile.width = 16;
        _baseProjectile.Projectile.height = 16;

        Gravity = 0.001f;
        GravityChange = 0.002f;
    }

    public Vector2 ApplyGravity(Vector2 velocity)
    {
        Gravity += GravityChange;
        velocity.Y += Gravity;

        return velocity;
    }
}