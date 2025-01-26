using Microsoft.Xna.Framework;
using Terraria;

namespace AquaRegia.Modules.Projectiles;

public interface IProjectileRuntime
{
    public virtual bool RuntimeTileCollide(BaseProjectile baseProjectile, Vector2 oldVelocity)
    {
        return true;
    }

    public virtual void RuntimeHitNPC(BaseProjectile baseProjectile, NPC target, NPC.HitInfo hit)
    {
    }

    public virtual void RuntimeAI(BaseProjectile baseProjectile)
    {
    }
}
