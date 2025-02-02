using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;

namespace AquaRegia.Modules.Projectiles;

public class ImmunityModule : IModule, IProjectileRuntime
{
    private Dictionary<NPC, int> _immunity = new();
    private List<NPC> _removeQueue = new();

    public int ImmunityTime { get; set; }

    public void SetDefaults(int time = 20)
    {
        ImmunityTime = time;
    }

    public void Reset(NPC target)
    {
        _immunity[target] = ImmunityTime;
    }

    public bool CanHit(NPC target)
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

    public void Update()
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

    public bool RuntimeTileCollide(BaseProjectile projectile, Vector2 oldVelocity)
    {
        return true;
    }

    public void RuntimeHitNPC(BaseProjectile projectile, NPC target, NPC.HitInfo hit)
    {
        Reset(target);
    }

    public void RuntimeAI(BaseProjectile projectile)
    {
        Update();
    }

    public void RuntimeKill(BaseProjectile projectile, int timeLeft)
    {
    }

    public bool RuntimeCanHitNPC(BaseProjectile projectile, NPC target)
    {
        return CanHit(target) ? true : false;
    }
}
