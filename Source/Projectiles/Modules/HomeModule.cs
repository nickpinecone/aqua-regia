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

    public Vector2? Update(Vector2 position, Vector2 velocity)
    {
        Target = Helper.FindNearsetNPC(position, Radius);

        if (Target != null)
        {
            var direction = Target.Center - position;
            var angle = Helper.AngleBetween(velocity, direction);

            var newVelocity = velocity.RotatedBy(MathF.Sign(angle) * MathF.Min(Curve, MathF.Abs(angle)));
            newVelocity.Normalize();
            newVelocity *= Speed;

            Curve *= CurveChange;

            return newVelocity;
        }

        return null;
    }

    public override void RuntimeAI(BaseProjectile baseProjectile)
    {
        base.RuntimeAI(baseProjectile);

        baseProjectile.Projectile.velocity =
            Update(baseProjectile.Projectile.Center, baseProjectile.Projectile.velocity) ??
            baseProjectile.Projectile.velocity;
    }
}
