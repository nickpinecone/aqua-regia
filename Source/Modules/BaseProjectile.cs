using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using AquaRegia.Modules.Projectiles;
using AquaRegia.Utils;
using static AquaRegia.AquaRegia;

namespace AquaRegia.Modules;

public abstract class BaseProjectile : ModProjectile
{
    protected AquaRegia.ProjectileSource _source = null;

    protected Dictionary<Type, BaseProjectileModule> _modules = new();
    protected List<BaseProjectileModule> _runtimeModules = new();
    protected ImmunityModule _immunity;

    public bool IsAmmoRuntime { get; set; }

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

    public T SpawnProjectile<T>(Vector2 position, Vector2 velocity, int damage, float knockback,
                                ProjectileSource customSource = null)
        where T : BaseProjectile
    {
        if (Projectile.owner != Main.myPlayer)
            return null;

        var type = ModContent.ProjectileType<T>();

        if (customSource != null)
        {
            customSource.Inherit(_source);
        }

        var proj = Projectile.NewProjectileDirect(customSource ?? _source, position, velocity, type, damage, knockback,
                                                  Projectile.owner);
        return (T)proj.ModProjectile;
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        _immunity.SetDefaults();
    }

    public override void OnSpawn(IEntitySource source)
    {
        base.OnSpawn(source);

        if (source is AquaRegia.ProjectileSource projSource)
        {
            _source = projSource;
        }
    }

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        bool isDefault = true;

        foreach (var module in _runtimeModules)
        {
            var status = module.RuntimeTileCollide(this, oldVelocity);
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
            module.RuntimeHitNPC(this, target, hit);
        }

        if (IsAmmoRuntime)
        {
            _source.Ammo.RuntimeHitNPC(target, hit);
        }

        _immunity.Reset(target);
    }

    public override void OnKill(int timeLeft)
    {
        base.OnKill(timeLeft);

        if (IsAmmoRuntime)
        {
            _source.Ammo.RuntimeKill();
        }
    }

    public override void AI()
    {
        base.AI();

        foreach (var module in _runtimeModules)
        {
            module.RuntimeAI(this);
        }

        _immunity.Update();
    }
}
