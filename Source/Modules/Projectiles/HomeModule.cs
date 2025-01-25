using System;
using Microsoft.Xna.Framework;
using Terraria;
using AquaRegia.Utils;

namespace AquaRegia.Modules.Projectiles;

public class HomeModule : BaseProjectileModule
{
    public NPC? Target { get; private set; }
    public float Curve { get; set; }
    public float CurveChange { get; set; }
    public float Speed { get; set; }
    public float Radius { get; set; }

    public HomeModule(BaseProjectile baseProjectile) : base(baseProjectile)
    {
    }

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

        var newVelocity = velocity.RotatedBy(MathF.Sign(angle) * MathF.Min(Curve, MathF.Abs(angle)));
        newVelocity.Normalize();
        newVelocity *= Speed;

        Curve *= CurveChange;

        return newVelocity;
    }

    public Vector2? Default(Vector2 position, Vector2 velocity, Func<NPC, bool>? canHome = null)
    {
        Target = Helper.FindNearestNPC(position, Radius, canHome);

        if (Target != null)
        {
            return Calculate(position, velocity, Target.Center);
        }

        return null;
    }

    public override void RuntimeAI(BaseProjectile baseProjectile)
    {
        base.RuntimeAI(baseProjectile);

        baseProjectile.Projectile.velocity =
            Default(baseProjectile.Projectile.Center, baseProjectile.Projectile.velocity) ??
            baseProjectile.Projectile.velocity;
    }
}
