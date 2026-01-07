using AquaRegia.Library.Extended.Extensions;
using Microsoft.Xna.Framework;
using Terraria;

namespace AquaRegia.Library.Extended.Modules.Projectiles;

public class GravityModule : IModule, IProjectileRuntime
{
    public float Default { get; private set; }
    public float Value { get; set; }
    public float Change { get; set; }

    public void SetDefaults(float gravity = 1f, float gravityChange = 1.01f)
    {
        Default = gravity;
        Value = Default;
        Change = gravityChange;
    }

    public Vector2 GetGravity(Vector2 velocity)
    {
        Value *= Change;
        velocity.Y += Value.ToSeconds();

        return velocity;
    }

    public void Reset()
    {
        Value = Default;
    }

    public void ApplyGravity(Projectile projectile)
    {
        projectile.velocity = GetGravity(projectile.velocity);
    }

    public void RuntimeAI(BaseProjectile baseProjectile)
    {
        ApplyGravity(baseProjectile.Projectile);
    }
}