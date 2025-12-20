using System;
using System.Collections.Generic;
using Terraria;

namespace AquaRegia.Library.Extended.Modules.Projectiles;

public class ImmunityModule : IModule, IProjectileRuntime
{
    private readonly Dictionary<NPC, int> _immunity = new();
    private readonly List<NPC> _removeQueue = new();

    private int ImmunityTime { get; set; }

    public ImmunityModule(int immunityTime = 20)
    {
        ImmunityTime = immunityTime;
    }

    private void Reset(NPC target)
    {
        _immunity[target] = ImmunityTime;
    }

    private bool CanHit(NPC target)
    {
        if (_immunity.ContainsKey(target))
        {
            return false;
        }
        else
        {
            _immunity[target] = 0;
            return true;
        }
    }

    private void Update()
    {
        foreach (var (npc, time) in _immunity)
        {
            _immunity[npc] = Math.Max(0, time - 1);

            if (_immunity[npc] == 0)
            {
                _removeQueue.Add(npc);
            }
        }

        foreach (var npc in _removeQueue)
        {
            _immunity.Remove(npc);
        }

        _removeQueue.Clear();
    }

    public void RuntimeOnHitNPC(BaseProjectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
    {
        Reset(target);
    }

    public void RuntimeAI(BaseProjectile projectile)
    {
        Update();
    }

    public bool RuntimeCanHitNPC(BaseProjectile projectile, NPC target)
    {
        return CanHit(target);
    }
}