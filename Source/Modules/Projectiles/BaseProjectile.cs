using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AquaRegia.Modules.Projectiles;

public abstract class BaseProjectile : ModProjectile, IComposite<IProjectileRuntime>
{
    private Dictionary<Type, IModule> _modules = new();
    Dictionary<Type, IModule> IComposite<IProjectileRuntime>.Modules => _modules;
    private List<IProjectileRuntime> _runtime = new();
    List<IProjectileRuntime> IComposite<IProjectileRuntime>.RuntimeModules => _runtime;

    public IComposite<IProjectileRuntime> Composite { get; init; }

    protected BaseProjectile()
    {
        Composite = ((IComposite<IProjectileRuntime>)this);
    }

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        base.OnTileCollide(oldVelocity);

        bool isDefault = true;

        foreach (var module in _runtime)
        {
            var status = module.RuntimeTileCollide(this, oldVelocity);

            if (status == false)
            {
                isDefault = false;
            }
        }

        return isDefault;
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        base.OnHitNPC(target, hit, damageDone);

        foreach (var module in _runtime)
        {
            module.RuntimeHitNPC(this, target, hit);
        }
    }

    public override void OnKill(int timeLeft)
    {
        base.OnKill(timeLeft);

        foreach (var module in _runtime)
        {
            module.RuntimeKill(this, timeLeft);
        }
    }

    public override void AI()
    {
        base.AI();

        foreach (var module in _runtime)
        {
            module.RuntimeAI(this);
        }
    }
}
