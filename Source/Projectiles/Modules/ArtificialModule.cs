using System;
using Microsoft.Xna.Framework;
using Terraria;
using WaterGuns.Utils;

namespace WaterGuns.Projectiles.Modules;

public class ArtificialModule : BaseProjectileModule
{
    public NPC Target { get; private set; }
    public float Curve { get; set; }
    public float CurveChange { get; set; }
    public float Speed { get; set; }
    public float Radius { get; set; }

    public ArtificialModule(BaseProjectile baseProjectile) : base(baseProjectile)
    {
    }

    public void SetDefaults()
    {
        Curve = 0.1f;
        CurveChange = 1.01f;
        Speed = 16;
        Radius = 300f;
    }

    public Vector2 AutoAim(Vector2 position, Vector2 velocity)
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

        return velocity;
    }

    public override void RuntimeAI()
    {
        base.RuntimeAI();

        _baseProjectile.Projectile.velocity = AutoAim(
            _baseProjectile.Projectile.Center, _baseProjectile.Projectile.velocity
        );
    }
}