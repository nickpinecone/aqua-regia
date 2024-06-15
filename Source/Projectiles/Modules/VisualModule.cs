using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using WaterGuns.Ammo;

namespace WaterGuns.Projectiles.Modules;

public class VisualModule : BaseProjectileModule
{
    public int DustAmount { get; set; }
    public float DustOffset { get; set; }
    public float DustScale { get; set; }
    public Color DustColor { get; set; }
    public int DustAlpha { get; set; }

    public VisualModule(BaseProjectile baseProjectile) : base(baseProjectile)
    {
    }

    public void SetDefaults()
    {
        DustAmount = 4;
        DustOffset = 3.4f;
        DustScale = 1f;
        DustColor = Color.White;
        DustAlpha = 0;
    }

    public void ApplyAmmo(BaseAmmo baseAmmo)
    {
        DustColor = baseAmmo.Color;
    }

    public void CreateDust(Vector2 position, Vector2 velocity)
    {
        var offset = new Vector2(velocity.X, velocity.Y);
        offset.Normalize();
        offset *= DustOffset;

        for (int i = 0; i < DustAmount; i++)
        {
            var newPosition = new Vector2(position.X + offset.X * i, position.Y + offset.Y * i);
            var dust = Dust.NewDustPerfect(newPosition, DustID.Wet, new Vector2(0, 0), DustAlpha, DustColor, DustScale);
            dust.noGravity = true;
            dust.fadeIn = 0f;
            dust.velocity = velocity.SafeNormalize(Vector2.Zero);
        }
    }
}