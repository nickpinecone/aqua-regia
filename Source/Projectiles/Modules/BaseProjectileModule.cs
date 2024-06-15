using Microsoft.Xna.Framework;
using Terraria;

namespace WaterGuns.Projectiles.Modules;

public abstract class BaseProjectileModule
{
    protected BaseProjectile _baseProjectile;

    protected BaseProjectileModule(BaseProjectile baseProjectile)
    {
        _baseProjectile = baseProjectile;
        _baseProjectile.AddModule(this);
    }

    public virtual bool RuntimeTileCollide(Vector2 oldVelocity)
    {
        return true;
    }

    public virtual void RuntimeHitNPC(NPC target, NPC.HitInfo hit)
    {
    }

    public virtual void RuntimeAI()
    {
    }
}