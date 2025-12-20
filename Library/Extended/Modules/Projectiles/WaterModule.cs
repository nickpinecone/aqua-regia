using System.Collections.Generic;
using AquaRegia.Library.Extended.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace AquaRegia.Library.Extended.Modules.Projectiles;

public class WaterModule : IModule, IProjectileRuntime
{
    public int Amount { get; set; }
    public float Offset { get; set; }
    public float Scale { get; set; }
    public Color Color { get; set; }
    public int Alpha { get; set; }
    public int Dust { get; set; }

    public void SetDefaults(int amount = 6, float offset = 3.8f, float scale = 1.2f, int alpha = 0,
        int dust = DustID.Wet, Color? color = null)
    {
        Amount = amount;
        Offset = offset;
        Scale = scale;
        Alpha = alpha;
        Dust = dust;
        Color = color ?? Color.White;
    }

    public void KillEffect(Vector2 position, Vector2 velocity)
    {
        velocity.Normalize();
        velocity *= 2f;

        DustHelper.Single(Dust, position, new Vector2(2, 2), velocity, 1.2f, 0, Color);
    }

    public List<Dust> CreateDust(Vector2 position, Vector2 velocity)
    {
        var offset = new Vector2(velocity.X, velocity.Y);
        offset.Normalize();
        offset *= Offset;

        var dusts = new List<Dust>();

        for (var i = 0; i < Amount; i++)
        {
            var offsetPosition = new Vector2(position.X + offset.X * i, position.Y + offset.Y * i);
            var particle = DustHelper.SinglePerfect(Dust, offsetPosition, Vector2.Zero, Scale, Alpha, Color);
            particle.noGravity = true;
            particle.fadeIn = 1f;
            particle.velocity = velocity.SafeNormalize(Vector2.Zero);

            dusts.Add(particle);
        }

        return dusts;
    }

    public void RuntimeOnKill(BaseProjectile projectile, int timeLeft)
    {
        KillEffect(projectile.Projectile.Center, projectile.Projectile.velocity);
    }

    public void RuntimeAI(BaseProjectile projectile)
    {
        CreateDust(projectile.Projectile.Center, projectile.Projectile.velocity);
    }
}