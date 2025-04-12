using Microsoft.Xna.Framework;
using Terraria;

namespace WaterGuns.Modules.Projectiles;

public interface IProjectileRuntime
{
    public abstract bool RuntimeTileCollide(BaseProjectile projectile, Vector2 oldVelocity);
    public abstract void RuntimeHitNPC(BaseProjectile projectile, NPC target, NPC.HitInfo hit);
    public abstract void RuntimeAI(BaseProjectile projectile);
    public abstract void RuntimeKill(BaseProjectile projectile, int timeLeft);
    public abstract bool RuntimeCanHitNPC(BaseProjectile projectile, NPC target);
}
