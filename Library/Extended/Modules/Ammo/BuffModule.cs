using System;
using AquaRegia.Library.Extended.Extensions;
using Terraria;

namespace AquaRegia.Library.Extended.Modules.Ammo;

public class BuffModule : IModule, IProjectileRuntime
{
    public int Buff { get; set; }
    public int Time { get; set; }

    private int _percent;

    private int Percent
    {
        get => _percent;
        set => _percent = Math.Clamp(value, 0, 100);
    }

    public void SetDefaults(int buff, int time, int percent)
    {
        Buff = buff;
        Time = time;
        Percent = percent;
    }

    public void ApplyBuff(NPC target)
    {
        if (Main.rand.Percent(Percent))
        {
            target.AddBuff(Buff, Time);
        }
    }

    public void RuntimeOnHitNPC(BaseProjectile baseProjectile, NPC target, NPC.HitInfo hit, int damageDone)
    {
        ApplyBuff(target);
    }
}