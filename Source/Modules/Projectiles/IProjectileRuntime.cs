using Microsoft.Xna.Framework;
using Terraria;

namespace AquaRegia.Modules.Projectiles;

public interface IProjectileRuntime
{
    public abstract bool RuntimeTileCollide(BaseProjectile baseProjectile, Vector2 oldVelocity);
    public abstract void RuntimeHitNPC(BaseProjectile baseProjectile, NPC target, NPC.HitInfo hit);
    public abstract void RuntimeAI(BaseProjectile baseProjectile);
    public abstract void RuntimeKill(BaseProjectile baseProjectile, int timeLeft);
}
