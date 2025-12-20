using System;
using Terraria;

namespace AquaRegia.Library.Extended.Modules.Ammo;

public class BuffModule : IModule, IProjectileRuntime
{
    private int _buff;
    private float _seconds;
    private int _percent;

    private int Percent
    {
        get => _percent;
        set => _percent = Math.Clamp(value, 0, 100);
    }

    public void SetDefaults(int buff, float seconds, int percent)
    {
        _buff = buff;
        _seconds = seconds;
        Percent = percent;
    }

    public void ApplyToTarget(NPC target)
    {
        if (Main.rand.Next(0, 100) < Percent)
        {
            target.AddBuff(_buff, (int)(_seconds * 60));
        }
    }

    public void RuntimeOnHitNPC(BaseProjectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
    {
        ApplyToTarget(target);
    }
}