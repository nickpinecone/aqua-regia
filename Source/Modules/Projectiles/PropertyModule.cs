using Microsoft.Xna.Framework;

namespace AquaRegia.Modules.Projectiles;

public class PropertyModule : IModule
{
    public float DefaultGravity { get; private set; }
    public int DefaultTime { get; private set; }

    public float Gravity { get; set; }
    public float GravityChange { get; set; }

    public void SetProperties(BaseProjectile projectile, int width = 0, int height = 0, int damage = 0,
                            int penetrate = 0, float knockBack = 0f, int alpha = 0, int critChance = 0,
                            bool tileCollide = true, bool hostile = false, bool friendly = true)
    {
        projectile.Projectile.width = width;
        projectile.Projectile.height = height;

        projectile.Projectile.damage = damage;
        projectile.Projectile.knockBack = knockBack;
        projectile.Projectile.penetrate = penetrate;
        projectile.Projectile.alpha = alpha;
        projectile.Projectile.CritChance = critChance;

        projectile.Projectile.tileCollide = tileCollide;
        projectile.Projectile.hostile = hostile;
        projectile.Projectile.friendly = friendly;

        projectile.Projectile.timeLeft = 0;
        DefaultTime = projectile.Projectile.timeLeft;
    }

    public void SetGravity(float gravity = 0.01f, float gravityChange = 0.02f)
    {
        DefaultGravity = gravity;
        Gravity = DefaultGravity;
        GravityChange = gravityChange;
    }

    public void SetTimeLeft(BaseProjectile projectile, int time = 0)
    {
        DefaultTime = time;
        projectile.Projectile.timeLeft = DefaultTime;
    }

    public Vector2 ApplyGravity(Vector2 velocity)
    {
        Gravity += GravityChange;
        velocity.Y += Gravity;

        return velocity;
    }
}
