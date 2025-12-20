using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;

namespace AquaRegia.Library.Extended.Modules;

public interface IProjectileRuntime
{
    public bool RuntimeTileCollide(BaseProjectile projectile, Vector2 oldVelocity)
    {
        return true;
    }

    public bool? RuntimeCanHitNPC(BaseProjectile projectile, NPC target)
    {
        return null;
    }

    public void RuntimeOnHitNPC(BaseProjectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
    {
    }

    public void RuntimeAI(BaseProjectile projectile)
    {
    }

    public void RuntimeOnKill(BaseProjectile projectile, int timeLeft)
    {
    }

    public void RuntimeOnSpawn(BaseProjectile baseProjectile, IEntitySource source)
    {
    }
}