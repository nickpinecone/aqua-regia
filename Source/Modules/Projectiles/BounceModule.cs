
using Microsoft.Xna.Framework;

namespace AquaRegia.Modules.Projectiles;

public class BounceModule : BaseProjectileModule
{
    public int MaxCount { get; set; }

    private PropertyModule? _property;
    private int _current;

    public BounceModule(BaseProjectile baseProjectile) : base(baseProjectile)
    {
        _property = null;
    }

    public void SetDefaults(PropertyModule? property = null, int maxCount = 3)
    {
        _property = property;
        MaxCount = maxCount;
    }

    public Vector2? Update(BaseProjectile baseProjectile, Vector2 oldVelocity, Vector2 velocity)
    {
        if (MaxCount == -1 || _current < MaxCount)
        {
            _current += 1;

            var newVelocity = velocity;
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

        var newVelocity = Update(baseProjectile, oldVelocity, baseProjectile.Projectile.velocity);
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
