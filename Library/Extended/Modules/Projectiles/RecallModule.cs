using System;
using Microsoft.Xna.Framework;
using Terraria;

namespace AquaRegia.Library.Extended.Modules.Projectiles;

public class RecallModule : IModule, IProjectileRuntime
{
    public float MaxSpeed { get; set; }
    public float Acc { get; set; }
    public float AccChange { get; set; }

    public float NearAmount { get; set; }
    public float FarThreshold { get; set; }
    public bool IsRecalled { get; private set; }

    public Action? OnRecall { get; set; }

    public void SetDefaults(float maxRecallSpeed, float acc = 1f, float accChange = 1.01f, float nearAmount = 32f,
        float farThreshold = 1000f)
    {
        MaxSpeed = maxRecallSpeed;
        Acc = acc;
        AccChange = accChange;
        NearAmount = nearAmount;
        FarThreshold = farThreshold;
    }

    public Vector2 GetRecallVelocity(Vector2 ownerPosition, Vector2 projectilePosition, Vector2 projectileVelocity)
    {
        var velocity = (ownerPosition - projectilePosition).SafeNormalize(Vector2.Zero) * MaxSpeed;
        return projectileVelocity.MoveTowards(velocity, Acc);
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

    public void RecallIfFar(Projectile projectile, Vector2 ownerPosition)
    {
        if (projectile.Center.DistanceSQ(ownerPosition) > Math.Pow(FarThreshold, 2))
        {
            Recall(projectile);
        }
    }

    public void Recall(Projectile projectile)
    {
        if (IsRecalled) return;

        IsRecalled = true;
        OnRecall?.Invoke();

        projectile.velocity = -projectile.velocity.SafeNormalize(Vector2.Zero);
    }

    public bool ApplyRecall(Projectile projectile, Player owner)
    {
        projectile.timeLeft = 10;

        RecallIfFar(projectile, owner.Center);

        if (IsRecalled)
        {
            projectile.tileCollide = false;
            projectile.velocity = GetRecallVelocity(owner.Center, projectile.Center, projectile.velocity);
            projectile.rotation = GetRecallRotation(projectile.velocity);

            Acc *= AccChange;
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