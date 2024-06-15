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

    public void AddRuntimeModule(BaseProjectileModule module)
    {
        if (!_modules.ContainsKey(module.GetType()))
        {
            _modules[module.GetType()] = module;
            _runtimeModules.Add(module);
        }
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

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        bool isDefault = true;

        foreach (var module in _runtimeModules)
        {
            var status = module.RuntimeTileCollide(oldVelocity);
            if (status == false)
            {
                isDefault = false;
            }
        }

        return isDefault;
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

        foreach (var module in _runtimeModules)
        {
            module.RuntimeHitNPC(target, hit);
        }

        _immunity.Reset(target);
    }

    public override void AI()
    {
        base.AI();

        foreach (var module in _runtimeModules)
        {
            module.RuntimeAI();
        }

        _immunity.Update();

        foreach (var timer in _timers)
        {
            timer.Update();
        }
    }
}