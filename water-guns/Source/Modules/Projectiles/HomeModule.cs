using System;
using Microsoft.Xna.Framework;
using Terraria;
using WaterGuns.Utils;

namespace WaterGuns.Modules.Projectiles;

public class HomeModule : IModule, IProjectileRuntime
{
    public NPC? Target { get; private set; }
    public float Curve { get; set; }
    public float CurveChange { get; set; }
    public float Speed { get; set; }
    public float Radius { get; set; }

    public void SetDefaults(float curve = 0.1f, float curveChange = 1.01f, int speed = 16, float radius = 300f)
    {
        Curve = curve;
        CurveChange = curveChange;
        Speed = speed;
        Radius = radius;
    }

    public Vector2 Calculate(Vector2 position, Vector2 velocity, Vector2 targetPosition)
    {
        var direction = targetPosition - position;

        if (velocity == Vector2.Zero)
        {
            velocity = direction;
        }

        var angle = Helper.AngleBetween(velocity, direction);

        Vector2 newVelocity;
        if (Curve == 0)
        {
            newVelocity = velocity.RotatedBy(angle);
            if (Speed != 0)
            {
                newVelocity.Normalize();
                newVelocity *= Speed;
            }
        }
        else
        {
            newVelocity = velocity.RotatedBy(MathF.Sign(angle) * MathF.Min(Curve, MathF.Abs(angle)));
            newVelocity.Normalize();
            newVelocity *= Speed;
        }

        Curve *= CurveChange;

        return newVelocity;
    }

    public Vector2? Update(Vector2 position, Vector2 velocity, Func<NPC, bool>? canHome = null)
    {
        Target = Helper.FindNearestNPC(position, Radius, canHome);

        if (Target != null)
        {
            return Calculate(position, velocity, Target.Center);
        }

        return null;
    }

    public void RuntimeAI(BaseProjectile projectile)
    {
        projectile.Projectile.velocity =
            Update(projectile.Projectile.Center, projectile.Projectile.velocity) ?? projectile.Projectile.velocity;
    }

    public bool RuntimeTileCollide(BaseProjectile projectile, Vector2 oldVelocity)
    {
        return true;
    }

    public void RuntimeHitNPC(BaseProjectile projectile, NPC target, NPC.HitInfo hit)
    {
    }

    public void RuntimeKill(BaseProjectile projectile, int timeLeft)
    {
    }

    public bool RuntimeCanHitNPC(BaseProjectile projectile, NPC target)
    {
        return true;
    }
}
