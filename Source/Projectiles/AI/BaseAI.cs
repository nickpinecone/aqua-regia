using System;
using Microsoft.Xna.Framework;

namespace WaterGuns.Projectiles.AI;

public enum AINames { NoAI, AutoAim };

public class AIData
{
    public Vector2? Position = null;
    public Vector2? Velocity = null;
    public float? Rotation = null;
    public float? Scale = null;

    public static AIData FromProjectile(BaseProjectile baseProjectile)
    {
        return new AIData()
        {
            Position = baseProjectile.Projectile.position,
            Velocity = baseProjectile.Projectile.velocity,
            Rotation = baseProjectile.Projectile.rotation,
            Scale = baseProjectile.Projectile.scale,
        };
    }

    public void ApplyToProjectile(BaseProjectile baseProjectile)
    {
        baseProjectile.Projectile.position = Position ?? baseProjectile.Projectile.position;
        baseProjectile.Projectile.velocity = Velocity ?? baseProjectile.Projectile.velocity;
        baseProjectile.Projectile.rotation = Rotation ?? baseProjectile.Projectile.rotation;
        baseProjectile.Projectile.scale = Scale ?? baseProjectile.Projectile.scale;
    }
}

public abstract class BaseAI
{
    public event EventHandler<BaseAI> OnSwitchAI;

    protected BaseProjectile _baseProjectile;

    public AINames Name { get; private set; }

    protected BaseAI(BaseProjectile baseProjectile, AINames aiName)
    {
        _baseProjectile = baseProjectile;
        Name = aiName;
    }

    public abstract AIData Update(AIData aiData);

    public void SwitchAI(BaseAI ai)
    {
        OnSwitchAI?.Invoke(this, ai);
    }
}