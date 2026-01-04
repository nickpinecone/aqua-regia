using System.Collections.Generic;
using AquaRegia.Library.Extended.Fluent;
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
        velocity = velocity.SafeNormalize(Vector2.Zero);
        velocity *= 2f;

        new DustSpawner(Dust).Single()
            .Position(position)
            .Size(new Vector2(2, 2), 1.2f)
            .Velocity(velocity)
            .Color(Color, 0)
            .Spawn();
    }

    public IEnumerable<Dust> CreateDust(Vector2 position, Vector2 velocity)
    {
        var offset = new Vector2(velocity.X, velocity.Y);
        offset = offset.SafeNormalize(Vector2.Zero);
        offset *= Offset;

        var dusts = new List<Dust>();
        for (var i = 0; i < Amount; i++)
        {
            var offsetPosition = new Vector2(position.X + offset.X * i, position.Y + offset.Y * i);

            var dust = new DustSpawner(Dust).Single()
                .Perfect(true)
                .Position(offsetPosition)
                .Size(Vector2.Zero, Scale)
                .Velocity(velocity.SafeNormalize(Vector2.Zero), true)
                .Color(Color, Alpha, 1f)
                .Spawn();

            dusts.Add(dust);
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