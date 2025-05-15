using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace WaterGuns.Library.Modules;

public abstract class BaseProjectile : ModProjectile, IComposite<IProjectileRuntime>
{
    public Dictionary<Type, IModule> Modules => [];
    public List<IProjectileRuntime> RuntimeModules => [];

    public IComposite<IProjectileRuntime> Composite { get; init; }
    public Player Owner => Main.player[Projectile.owner];

    protected BaseProjectile()
    {
        Composite = this;
    }

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        var defaultValue = base.OnTileCollide(oldVelocity);
        bool? custom = null;

        foreach (var module in RuntimeModules)
        {
            var value = module.RuntimeTileCollide(this, oldVelocity);

            if (value != defaultValue)
            {
                custom = value;
            }
        }

        return custom ?? defaultValue;
    }

    public override bool? CanHitNPC(NPC target)
    {
        var defaultValue = base.CanHitNPC(target);
        bool? custom = null;

        foreach (var module in RuntimeModules)
        {
            var value = module.RuntimeCanHitNPC(this, target);

            if (value != defaultValue)
            {
                custom = value;
            }
        }

        return custom ?? defaultValue;
    }

    public override void OnSpawn(IEntitySource source)
    {
        base.OnSpawn(source);

        foreach (var module in RuntimeModules)
        {
            module.RuntimeOnSpawn(this, source);
        }
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        base.OnHitNPC(target, hit, damageDone);

        foreach (var module in RuntimeModules)
        {
            module.RuntimeOnHitNPC(this, target, hit, damageDone);
        }
    }

    public override void OnKill(int timeLeft)
    {
        base.OnKill(timeLeft);

        foreach (var module in RuntimeModules)
        {
            module.RuntimeOnKill(this, timeLeft);
        }
    }

    public override void AI()
    {
        base.AI();

        foreach (var module in RuntimeModules)
        {
            module.RuntimeAI(this);
        }
    }
}

public interface IProjectileRuntime
{
    public bool RuntimeTileCollide(BaseProjectile projectile, Vector2 oldVelocity)
    {
        return false;
    }

    public bool RuntimeCanHitNPC(BaseProjectile projectile, NPC target)
    {
        return true;
    }

    public void RuntimeOnHitNPC(BaseProjectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
    {
    }

    public void RuntimeAI(BaseProjectile projectile)
    {
    }

    public void RuntimeOnKill(BaseProjectile projectile, int timeLeft)
    {
    }

    public void RuntimeOnSpawn(BaseProjectile baseProjectile, IEntitySource source)
    {
    }
}