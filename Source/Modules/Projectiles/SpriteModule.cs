using System;
using Microsoft.Xna.Framework;
using Terraria;

namespace AquaRegia.Modules.Projectiles;

public class SpriteModule : IModule
{
    public void Animate(BaseProjectile projectile, int delay)
    {
        projectile.Projectile.frameCounter += 1;

        if (projectile.Projectile.frameCounter >= delay)
        {
            projectile.Projectile.frameCounter = 0;
            projectile.Projectile.frame += 1;

            if (projectile.Projectile.frame >= Main.projFrames[projectile.Projectile.type])
            {
                projectile.Projectile.frame = 0;
            }
        }
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
