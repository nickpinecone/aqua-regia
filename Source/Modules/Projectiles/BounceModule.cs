using Microsoft.Xna.Framework;
using Terraria;

namespace AquaRegia.Modules.Projectiles;

public class BounceModule : IModule, IProjectileRuntime
{
    private int _current;

    public int MaxCount { get; set; }

    public void SetDefaults(int maxCount = 3)
    {
        MaxCount = maxCount;
    }

    public Vector2? Calculate(Vector2 oldVelocity, Vector2 velocity)
    {
        if (MaxCount == -1 || _current < MaxCount)
        {
            _current += 1;

            var newVelocity = velocity;
            if (oldVelocity.X != velocity.X)
                newVelocity.X = -oldVelocity.X;
            if (oldVelocity.Y != velocity.Y)
                newVelocity.Y = -oldVelocity.Y;

            return newVelocity;
        }

        return null;
    }

    public bool RuntimeTileCollide(BaseProjectile projectile, Vector2 oldVelocity)
    {
        var newVelocity = Calculate(oldVelocity, projectile.Projectile.velocity);

        if (newVelocity != null)
        {
            projectile.Projectile.velocity = (Vector2)newVelocity;
            return false;
        }
        else
        {
            return true;
        }
    }

    public void RuntimeHitNPC(BaseProjectile projectile, NPC target, NPC.HitInfo hit)
    {
    }

    public void RuntimeAI(BaseProjectile projectile)
    {
    }

    public void RuntimeKill(BaseProjectile projectile, int timeLeft)
    {
    }

    public bool RuntimeCanHitNPC(BaseProjectile projectile, NPC target)
    {
        return true;
    }
}
