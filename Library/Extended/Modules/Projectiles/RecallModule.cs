using System;
using Microsoft.Xna.Framework;
using Terraria;

namespace AquaRegia.Library.Extended.Modules.Projectiles;

public class RecallModule : IModule, IProjectileRuntime
{
    public float RecallSpeed { get; set; }
    public float RecallAcc { get; set; }
    public float NearAmount { get; set; }

    public bool IsRecalled { get; private set; }

    public void SetDefaults(float recallSpeed, float recallAcc = 1f, float nearAmount = 32f)
    {
        RecallSpeed = recallSpeed;
        RecallAcc = recallAcc;
        NearAmount = nearAmount;
    }

    public Vector2 GetRecallVelocity(Vector2 ownerPosition, Vector2 projectilePosition, Vector2 projectileVelocity)
    {
        var velocity = (ownerPosition - projectilePosition).SafeNormalize(Vector2.Zero) * RecallSpeed;
        return projectileVelocity.MoveTowards(velocity, RecallAcc);
    }

    public float GetRecallRotation(Vector2 velocity)
    {
        return velocity.ToRotation() + MathHelper.ToRadians(-45f);
    }

    public void KillWhenNear(Projectile projectile, Vector2 ownerPosition)
    {
        if (ownerPosition.DistanceSQ(projectile.Center) < Math.Pow(NearAmount, 2))
        {
            projectile.Kill();
        }
    }

    public void Recall(Projectile projectile)
    {
        if (IsRecalled) return;
        IsRecalled = true;

        projectile.velocity = -projectile.velocity.SafeNormalize(Vector2.Zero);
    }

    public bool ApplyRecall(Projectile projectile, Player owner)
    {
        projectile.timeLeft = 10;

        if (IsRecalled)
        {
            projectile.tileCollide = false;
            projectile.velocity = GetRecallVelocity(owner.Center, projectile.Center, projectile.velocity);
            projectile.rotation = GetRecallRotation(projectile.velocity);

            RecallAcc *= 1.01f;
            KillWhenNear(projectile, owner.Center);

            return false;
        }

        return true;
    }

    public bool RuntimePreAI(BaseProjectile baseProjectile)
    {
        return ApplyRecall(baseProjectile.Projectile, baseProjectile.Owner);
    }
}