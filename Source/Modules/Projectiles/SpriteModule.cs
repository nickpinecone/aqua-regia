using System;
using Microsoft.Xna.Framework;

namespace AquaRegia.Modules.Projectiles;

public class SpriteModule : BaseProjectileModule
{
    public SpriteModule(BaseProjectile baseProjectile) : base(baseProjectile)
    {
    }

    public float RotateOnMove(Vector2 velocity, float amount)
    {
        if (Math.Abs(velocity.X) > 0)
        {
            return amount * Math.Sign(velocity.X);
        }
        else
        {
            return 0f;
        }
    }
}
