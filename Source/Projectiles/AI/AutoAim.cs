using System;
using Microsoft.Xna.Framework;
using Terraria;
using WaterGuns.Utils;

namespace WaterGuns.Projectiles.AI;

public class AutoAim : BaseAI
{
    public NPC Target { get; private set; }
    public float Curve { get; set; }
    public float CurveChange { get; set; }
    public float Speed { get; set; }
    public float Radius { get; set; }

    public AutoAim(BaseProjectile baseProjectile)
        : base(baseProjectile, AINames.AutoAim)
    {
    }

    public void SetDefaults()
    {
        Curve = 0.1f;
        CurveChange = 1.01f;
        Speed = 16;
        Radius = 300f;
    }

    public override AIData Update(AIData aiData)
    {
        var position = (Vector2)aiData.Position;
        var velocity = (Vector2)aiData.Velocity;

        var result = new AIData();
        result.Velocity = velocity;

        Target = Helper.FindNearsetNPC(position, Radius);

        if (Target != null)
        {
            var direction = Target.Center - position;
            var angle = Helper.AngleBetween(velocity, direction);

            var newVelocity = velocity.RotatedBy(MathF.Sign(angle) * MathF.Min(Curve, MathF.Abs(angle)));
            newVelocity.Normalize();
            newVelocity *= Speed;

            Curve *= CurveChange;

            result.Velocity = newVelocity;
        }

        return result;
    }
}