using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;

namespace AquaRegia.Library.Extended.Modules;

public interface IProjectileRuntime
{
    public bool RuntimeTileCollide(BaseProjectile baseProjectile, Vector2 oldVelocity)
    {
        return true;
    }

    public bool? RuntimeCanHitNPC(BaseProjectile baseProjectile, NPC target)
    {
        return null;
    }

    public void RuntimeOnHitNPC(BaseProjectile baseProjectile, NPC target, NPC.HitInfo hit, int damageDone)
    {
    }

    public void RuntimeAI(BaseProjectile baseProjectile)
    {
    }

    public void RuntimeOnKill(BaseProjectile baseProjectile, int timeLeft)
    {
    }

    public void RuntimeOnSpawn(BaseProjectile baseProjectile, IEntitySource source)
    {
    }

    public bool RuntimePreAI(BaseProjectile baseProjectile)
    {
        return true;
    }
}