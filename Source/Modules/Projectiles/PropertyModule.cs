using Microsoft.Xna.Framework;
using Terraria;

namespace AquaRegia.Modules.Projectiles;

public class PropertyModule : BaseProjectileModule
{
    public float DefaultGravity { get; private set; }
    public int DefaultTime { get; private set; }

    public float Gravity { get; set; }
    public float GravityChange { get; set; }

    public PropertyModule(BaseProjectile baseProjectile) : base(baseProjectile)
    {
    }

    public void SetDefaults(BaseProjectile baseProjectile, int width = 0, int height = 0, int damage = 0,
                            int penetrate = 0, float knockBack = 0f, int alpha = 0, int critChance = 0,
                            bool tileCollide = true, bool hostile = false, bool friendly = true)
    {
        baseProjectile.Projectile.width = width;
        baseProjectile.Projectile.height = height;

        baseProjectile.Projectile.damage = damage;
        baseProjectile.Projectile.knockBack = knockBack;
        baseProjectile.Projectile.penetrate = penetrate;
        baseProjectile.Projectile.alpha = alpha;
        baseProjectile.Projectile.CritChance = critChance;

        baseProjectile.Projectile.tileCollide = tileCollide;
        baseProjectile.Projectile.hostile = hostile;
        baseProjectile.Projectile.friendly = friendly;

        baseProjectile.Projectile.timeLeft = 0;
        DefaultTime = baseProjectile.Projectile.timeLeft;
    }

    public void SetGravity(float gravity = 0.01f, float gravityChange = 0.02f)
    {
        DefaultGravity = gravity;
        Gravity = DefaultGravity;
        GravityChange = gravityChange;
    }

    public void SetTimeLeft(BaseProjectile baseProjectile, int time = 0)
    {
        DefaultTime = time;
        baseProjectile.Projectile.timeLeft = DefaultTime;
    }

    public Vector2 ApplyGravity(Vector2 velocity)
    {
        Gravity += GravityChange;
        velocity.Y += Gravity;

        return velocity;
    }

    public void AnimateSprite(BaseProjectile baseProjectile, int delay)
    {
        baseProjectile.Projectile.frameCounter += 1;

        if (baseProjectile.Projectile.frameCounter >= delay)
        {
            baseProjectile.Projectile.frameCounter = 0;
            baseProjectile.Projectile.frame += 1;

            if (baseProjectile.Projectile.frame >= Main.projFrames[baseProjectile.Projectile.type])
            {
                baseProjectile.Projectile.frame = 0;
            }
        }
    }
}
