using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using AquaRegia.Utils;
using System.Collections.Generic;

namespace AquaRegia.Modules.Projectiles;

public class WaterModule : BaseProjectileModule
{
    public int Amount { get; set; }
    public float Offset { get; set; }
    public float Scale { get; set; }
    public Color Color { get; set; }
    public int Alpha { get; set; }
    public int ParticleID { get; set; }

    public WaterModule(BaseProjectile baseProjectile) : base(baseProjectile)
    {
    }

    public void SetDefaults(int amount = 6, float offset = 3.8f, float scale = 1.2f, int alpha = 0,
                            int particle = DustID.Wet, Color? color = null)
    {
        Amount = amount;
        Offset = offset;
        Scale = scale;
        Alpha = alpha;
        ParticleID = particle;
        Color = color ?? Color.White;
    }

    public void ApplyAmmo(BaseAmmo? baseAmmo)
    {
        if (baseAmmo is not null)
        {
            Color = baseAmmo.AccentColor;
        }
    }

    public void KillEffect(Vector2 position, Vector2 velocity)
    {
        velocity.Normalize();
        velocity *= 2f;

        Particle.Single(ParticleID, position, new Vector2(2, 2), velocity, 1.2f, 0, Color);
    }

    public List<Dust> CreateDust(Vector2 position, Vector2 velocity)
    {
        var offset = new Vector2(velocity.X, velocity.Y);
        offset.Normalize();
        offset *= Offset;

        var dusts = new List<Dust>();

        for (int i = 0; i < Amount; i++)
        {
            var newPosition = new Vector2(position.X + offset.X * i, position.Y + offset.Y * i);
            var particle = Particle.SinglePerfect(ParticleID, newPosition, Vector2.Zero, Scale, Alpha, Color);
            particle.noGravity = true;
            particle.fadeIn = 1f;
            particle.velocity = velocity.SafeNormalize(Vector2.Zero);

            dusts.Add(particle);
        }

        return dusts;
    }
}
