using System;
using Microsoft.Xna.Framework;

namespace AquaRegia.Library.Extended.Modules.Projectiles;

public class RotateOnMoveModule : IModule, IProjectileRuntime
{
    public float Amount { get; set; }

    public void SetDefaults(float amount = 0.1f)
    {
        Amount = amount;
    }

    public float GetRotation(Vector2 velocity)
    {
        if (Math.Abs(velocity.X) > 0)
        {
            return Amount * Math.Sign(velocity.X);
        }

        return 0f;
    }

    public void RuntimeAI(BaseProjectile projectile)
    {
        projectile.Projectile.rotation += GetRotation(projectile.Projectile.velocity);
    }
}