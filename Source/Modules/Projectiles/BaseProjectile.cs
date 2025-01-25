using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using AquaRegia.Modules.Projectiles;
using static AquaRegia.AquaRegia;

namespace AquaRegia.Modules;

public abstract class BaseProjectile : ModProjectile, IComposite<BaseProjectileModule>
{
    protected AquaRegia.ProjectileSource? _source = null;

    protected Dictionary<Type, BaseProjectileModule> _modules = new();
    Dictionary<Type, BaseProjectileModule> IComposite<BaseProjectileModule>.Modules => _modules;
    protected List<BaseProjectileModule> _runtimeModules = new();
    List<BaseProjectileModule> IComposite<BaseProjectileModule>.RuntimeModules => _runtimeModules;
    protected ImmunityModule _immunity;

    public bool IsAmmoRuntime { get; set; }

    protected BaseProjectile()
    {
        _immunity = new ImmunityModule(this);
    }

    public T? SpawnProjectile<T>(Vector2 position, Vector2 velocity, int damage, float knockback,
                                 ProjectileSource? customSource = null)
        where T : BaseProjectile
    {
        if (Projectile.owner != Main.myPlayer)
            return null;

        var type = ModContent.ProjectileType<T>();

        if (customSource is not null && _source is not null)
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
            _source?.Ammo?.RuntimeHitNPC(target, hit);
        }

        _immunity.Reset(target);
    }

    public override void OnKill(int timeLeft)
    {
        base.OnKill(timeLeft);

        if (IsAmmoRuntime)
        {
            _source?.Ammo?.RuntimeKill();
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
