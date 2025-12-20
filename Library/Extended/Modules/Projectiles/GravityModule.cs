using Microsoft.Xna.Framework;

namespace AquaRegia.Library.Extended.Modules.Projectiles;

public class GravityModule : IModule, IProjectileRuntime
{
    public float DefaultGravity { get; private set; }
    public float Gravity { get; set; }
    public float GravityChange { get; set; }

    public void SetDefaults(float gravity = 0.01f, float gravityChange = 0.02f)
    {
        DefaultGravity = gravity;
        Gravity = DefaultGravity;
        GravityChange = gravityChange;
    }

    public Vector2 ApplyGravity(Vector2 velocity)
    {
        Gravity += GravityChange;
        velocity.Y += Gravity;

        return velocity;
    }

    public void RuntimeAI(BaseProjectile projectile)
    {
        projectile.Projectile.velocity = ApplyGravity(projectile.Projectile.velocity);
    }
}