using System;
using Microsoft.Xna.Framework;
using Terraria;
using WaterGuns.Utils;

namespace WaterGuns.Projectiles.Modules;

// TODO Experiment with https://github.com/tModLoader/tModLoader/wiki/Geometry#vector2torotation

public class HomeModule : BaseProjectileModule
{
    public NPC Target { get; private set; }
    public float Curve { get; set; }
    public float CurveChange { get; set; }
    public float Speed { get; set; }
    public float Radius { get; set; }

    public HomeModule(BaseProjectile baseProjectile) : base(baseProjectile)
    {
    }

    public void SetDefaults()
    {
        Curve = 0.1f;
        CurveChange = 1.01f;
        Speed = 16;
        Radius = 300f;
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

    public Vector2? Default(Vector2 position, Vector2 velocity, Func<NPC, bool> canHome = null)
    {
        Target = Helper.FindNearsetNPC(position, Radius, canHome);

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
