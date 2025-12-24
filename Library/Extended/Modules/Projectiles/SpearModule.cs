using Microsoft.Xna.Framework;
using Terraria;

namespace AquaRegia.Library.Extended.Modules.Projectiles;

public class SpearModule : IModule, IProjectileRuntime
{
    public float HoldoutRangeMin { get; set; }
    public float HoldoutRangeMax { get; set; }

    public void SetDefaults(float holdoutRangeMin = 24f, float holdoutRangeMax = 96f)
    {
        HoldoutRangeMin = holdoutRangeMin;
        HoldoutRangeMax = holdoutRangeMax;
    }

    public void RuntimeAI(BaseProjectile baseProjectile)
    {
        var owner = baseProjectile.Owner;
        var projectile = baseProjectile.Projectile;

        owner.heldProj = projectile.whoAmI;
        var duration = owner.itemAnimationMax;

        if (projectile.timeLeft > duration)
        {
            projectile.timeLeft = duration;
        }

        projectile.velocity = projectile.velocity.SafeNormalize(Vector2.Zero);

        var halfDuration = duration * 0.5f;
        float progress;

        if (projectile.timeLeft < halfDuration)
        {
            progress = projectile.timeLeft / halfDuration;
        }
        else
        {
            progress = (duration - projectile.timeLeft) / halfDuration;
        }

        projectile.Center = owner.MountedCenter + Vector2.SmoothStep(
            projectile.velocity * HoldoutRangeMin,
            projectile.velocity * HoldoutRangeMax, progress
        );

        projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(135f);
    }
}