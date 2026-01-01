using System;
using Microsoft.Xna.Framework;
using Terraria;

namespace AquaRegia.Library.Extended.Modules.Projectiles;

public class RecallModule : IModule, IProjectileRuntime
{
    public float RecallSpeed { get; set; }
    public bool Recalled { get; private set; }

    public void SetDefaults(float recallSpeed)
    {
        RecallSpeed = recallSpeed;
    }

    public void Recall()
    {
        Recalled = true;
    }

    public void Update(BaseProjectile baseProjectile)
    {
        var projectile = baseProjectile.Projectile;
        var owner = baseProjectile.Owner;

        projectile.timeLeft = 10;
        if (!Recalled) return;

        projectile.tileCollide = false;
        var velocity = (owner.Center - projectile.Center).SafeNormalize(Vector2.Zero) * RecallSpeed;
        projectile.velocity = projectile.velocity.MoveTowards(velocity, 1f);

        if (owner.Center.DistanceSQ(projectile.Center) < Math.Pow(32f, 2))
        {
            projectile.Kill();
        }

        projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(-45f);
    }

    public bool RuntimePreAI(BaseProjectile projectile)
    {
        Update(projectile);

        return !Recalled;
    }
}