using System;
using AquaRegia.Modules.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;

namespace AquaRegia.Modules.Ammo;

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

    public void RuntimeAI(BaseProjectile baseProjectile)
    {
    }

    public void RuntimeHitNPC(BaseProjectile baseProjectile, NPC target, NPC.HitInfo hit)
    {
        if (Main.rand.Next(0, 100) < Percent)
        {
            target.AddBuff(BuffID, (int)(Seconds * 60));
        }
    }

    public void RuntimeKill(BaseProjectile baseProjectile, int timeLeft)
    {
    }

    public bool RuntimeTileCollide(BaseProjectile baseProjectile, Vector2 oldVelocity)
    {
        return true;
    }
}
