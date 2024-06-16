
using System;
using System.Collections.Generic;
using Terraria;

namespace WaterGuns.Projectiles.Modules;

public class ImmunityModule : BaseProjectileModule
{
    private Dictionary<NPC, int> _immunity = new();

    public int ImmunityTime { get; set; }

    public ImmunityModule(BaseProjectile baseProjectile) : base(baseProjectile)
    {
    }

    public void SetDefaults()
    {
        ImmunityTime = 20;
    }

    public void Reset(NPC target)
    {
        _immunity[target] = ImmunityTime;
    }

    public bool CanHit(NPC target)
    {
        if (_immunity.ContainsKey(target))
        {
            return _immunity[target] <= 0;
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
        }
    }
}