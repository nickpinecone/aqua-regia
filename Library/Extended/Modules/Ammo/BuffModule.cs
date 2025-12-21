using System;
using AquaRegia.Library.Extended.Extensions;
using Terraria;

namespace AquaRegia.Library.Extended.Modules.Ammo;

public class BuffModule : IModule, IProjectileRuntime
{
    public int Buff { get; set; }
    public float Seconds { get; set; }

    private int _percent;

    private int Percent
    {
        get => _percent;
        set => _percent = Math.Clamp(value, 0, 100);
    }

    public void SetDefaults(int buff, float seconds, int percent)
    {
        Buff = buff;
        Seconds = seconds;
        Percent = percent;
    }

    public void ApplyToTarget(NPC target)
    {
        if (Main.rand.Percent(Percent))
        {
            target.AddBuff(Buff, (int)(Seconds * 60));
        }
    }

    public void RuntimeOnHitNPC(BaseProjectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
    {
        ApplyToTarget(target);
    }
}