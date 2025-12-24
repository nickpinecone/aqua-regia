using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AquaRegia.Library.Extended.Modules;

public abstract class BaseProjectile : ModProjectile, IComposite<IProjectileRuntime>
{
    public Dictionary<Type, IModule> Modules { get; } = [];
    public List<IProjectileRuntime> RuntimeModules { get; } = [];

    public IComposite<IProjectileRuntime> Composite { get; init; }
    public Player Owner => Main.player[Projectile.owner];

    protected BaseProjectile()
    {
        Composite = this;
        Composite.AddModules();
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
    
    public override bool PreAI()
    {
        var result = base.PreAI();

        foreach (var module in RuntimeModules)
        {
            result &= module.RuntimePreAI(this);
        }

        return result;
    }
}