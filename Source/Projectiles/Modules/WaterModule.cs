using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using WaterGuns.Ammo;
using WaterGuns.Utils;

namespace WaterGuns.Projectiles.Modules;

public class WaterModule : BaseProjectileModule
{
    public int Amount { get; set; }
    public float Offset { get; set; }
    public float Scale { get; set; }
    public Color Color { get; set; }
    public int Alpha { get; set; }

    public WaterModule(BaseProjectile baseProjectile) : base(baseProjectile)
    {
    }

    public void SetDefaults()
    {
        Amount = 6;
        Offset = 3.8f;
        Scale = 1.2f;
        Alpha = 0;
        Color = Color.White;
    }

    public void ApplyAmmo(BaseAmmo baseAmmo)
    {
        Color = baseAmmo.Color;
    }

    public void KillEffect(Vector2 position, Vector2 velocity)
    {
        velocity.Normalize();
        velocity *= 2f;

        Particle.Single(DustID.Wet, position, new Vector2(2, 2), velocity, 1.2f);
    }

    public void CreateDust(Vector2 position, Vector2 velocity)
    {
        var offset = new Vector2(velocity.X, velocity.Y);
        offset.Normalize();
        offset *= Offset;

        for (int i = 0; i < Amount; i++)
        {
            var newPosition = new Vector2(position.X + offset.X * i, position.Y + offset.Y * i);
            var particle = Particle.SinglePerfect(DustID.Wet, newPosition, Vector2.Zero, Scale, Alpha, Color);
            particle.noGravity = true;
            particle.fadeIn = 1f;
            particle.velocity = velocity.SafeNormalize(Vector2.Zero);
        }
    }
}
