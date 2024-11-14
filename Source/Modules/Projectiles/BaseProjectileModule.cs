using Microsoft.Xna.Framework;
using Terraria;

namespace AquaRegia.Modules.Projectiles;

public abstract class BaseProjectileModule
{
    protected BaseProjectileModule(IComposite<BaseProjectileModule> baseProjectile)
    {
        baseProjectile.AddModule(this);
    }

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
