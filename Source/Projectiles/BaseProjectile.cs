using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using WaterGuns.Projectiles.Modules;
using WaterGuns.Utils;

namespace WaterGuns.Projectiles;

public abstract class BaseProjectile : ModProjectile
{
    private List<Timer> _timers = new();
    protected WaterGuns.ProjectileSource _source = null;

    protected Dictionary<Type, BaseProjectileModule> _modules = new();
    protected List<BaseProjectileModule> _runtimeModules = new();

    protected ImmunityModule _immunity;

    protected BaseProjectile()
    {
        _immunity = new ImmunityModule(this);
    }

    public bool HasModule<T>()
    {
        return _modules.ContainsKey(typeof(T));
    }

    public void AddModule(BaseProjectileModule module)
    {
        _modules[module.GetType()] = module;
    }

    public T GetModule<T>()
        where T : BaseProjectileModule
    {
        return (T)_modules[typeof(T)];
    }

    public T SpawnProjectile<T>(
        Vector2 position, Vector2 velocity, int damage, int knockback
    ) where T : BaseProjectile
    {
        if (Projectile.owner != Main.myPlayer) return null;

        var type = ModContent.ProjectileType<T>();

        var proj = Projectile.NewProjectileDirect(_source, position, velocity, type, damage, knockback, Projectile.owner);
        return (T)proj.ModProjectile;
    }

    public override void OnSpawn(IEntitySource source)
    {
        base.OnSpawn(source);

        _source = (WaterGuns.ProjectileSource)source;
    }

    public override bool? CanHitNPC(NPC target)
    {
        if (_immunity.CanHit(target))
        {
            return base.CanHitNPC(target);
        }
        else
        {
            return false;
        }
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        base.OnHitNPC(target, hit, damageDone);

        _immunity.Reset(target);
    }

    public override void AI()
    {
        base.AI();

        _immunity.Update();

        foreach (var timer in _timers)
        {
            timer.Update();
        }
    }
}