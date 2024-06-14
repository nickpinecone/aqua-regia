using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using WaterGuns.Projectiles.Modules;

namespace WaterGuns.Projectiles;

public abstract class BaseProjectile : ModProjectile
{
    private WaterGuns.ProjectileSource _source = null;

    private Dictionary<Type, BaseProjectileModule> _modules = new();
    private List<BaseProjectileModule> _runtimeModules = new();

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
        var type = ModContent.ProjectileType<T>();

        var proj = Projectile.NewProjectileDirect(_source, position, velocity, type, damage, knockback, Projectile.owner);
        return (T)proj.ModProjectile;
    }

    public override void OnSpawn(IEntitySource source)
    {
        base.OnSpawn(source);

        _source = (WaterGuns.ProjectileSource)source;
    }
}