using System;
using WaterGuns.Modules.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;

namespace WaterGuns.Modules.Ammo;

public class BuffModule : IModule, IProjectileRuntime
{
    public int BuffID { get; set; }
    public float Seconds { get; set; }

    private int _percent;
    public int Percent
    {
        get
        {
            return _percent;
        }
        set
        {
            _percent = Math.Clamp(value, 0, 100);
        }
    }

    public void SetDefaults(int buffId, float seconds, int percent)
    {
        BuffID = buffId;
        Seconds = seconds;
        Percent = percent;
    }

    public void RuntimeHitNPC(BaseProjectile projectile, NPC target, NPC.HitInfo hit)
    {
        if (Main.rand.Next(0, 100) < Percent)
        {
            target.AddBuff(BuffID, (int)(Seconds * 60));
        }
    }

    public void RuntimeAI(BaseProjectile projectile)
    {
    }

    public void RuntimeKill(BaseProjectile projectile, int timeLeft)
    {
    }

    public bool RuntimeTileCollide(BaseProjectile projectile, Vector2 oldVelocity)
    {
        return true;
    }

    public bool RuntimeCanHitNPC(BaseProjectile projectile, NPC target)
    {
        return true;
    }
}
