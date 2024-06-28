
using Microsoft.Xna.Framework;

namespace WaterGuns.Projectiles.Modules;

public class BounceModule : BaseProjectileModule
{
    private PropertyModule _property;
    private int _current;
    public int BounceCount { get; set; }

    public BounceModule(BaseProjectile baseProjectile, PropertyModule property) : base(baseProjectile)
    {
        _property = property;
    }

    public void SetDefaults()
    {
        BounceCount = 3;
    }

    public Vector2? Bounce(BaseProjectile baseProjectile, Vector2 oldVelocity, Vector2 velocity)
    {
        if (_current < BounceCount)
        {
            _current += 1;

            var newVelocity = Vector2.Zero;
            if (oldVelocity.X != velocity.X)
                newVelocity.X = -oldVelocity.X;
            if (oldVelocity.Y != velocity.Y)
                newVelocity.Y = -oldVelocity.Y;

            if (_property != null)
            {
                baseProjectile.Projectile.timeLeft = _property.DefaultTime;
                _property.Gravity = _property.DefaultGravity;
            }

            return newVelocity;
        }
        return null;
    }

    public override bool RuntimeTileCollide(BaseProjectile baseProjectile, Vector2 oldVelocity)
    {
        base.RuntimeTileCollide(baseProjectile, oldVelocity);

        var newVelocity = Bounce(baseProjectile, oldVelocity, baseProjectile.Projectile.velocity);
        if (newVelocity != null)
        {
            baseProjectile.Projectile.velocity = (Vector2)newVelocity;
            return false;
        }
        else
        {
            return true;
        }
    }
}
