using Microsoft.Xna.Framework;

namespace AquaRegia.Library.Extended.Modules.Projectiles;

public class GravityModule : IModule, IProjectileRuntime
{
    public float Default { get; private set; }
    public float Value { get; set; }
    public float Change { get; set; }

    public void SetDefaults(float gravity = 0.01f, float gravityChange = 0.02f)
    {
        Default = gravity;
        Value = Default;
        Change = gravityChange;
    }

    public Vector2 ApplyGravity(Vector2 velocity)
    {
        Value += Change;
        velocity.Y += Value;

        return velocity;
    }

    public void RuntimeAI(BaseProjectile projectile)
    {
        projectile.Projectile.velocity = ApplyGravity(projectile.Projectile.velocity);
    }
}