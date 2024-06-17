using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WaterGuns.Ammo;

namespace WaterGuns.Projectiles.Modules;

public class DustData
{
    public int Amount;
    public float Offset;
    public float Scale;
    public Color Color;
    public int Alpha;
}

public class ChainData
{
    public string TexturePath;
    public Rectangle Source;
}

public class VisualModule : BaseProjectileModule
{
    public DustData DustData { get; set; }
    public ChainData ChainData { get; set; }

    public VisualModule(BaseProjectile baseProjectile) : base(baseProjectile)
    {
    }

    public void SetDefaults()
    {
        DustData = new DustData()
        {
            Amount = 4,
            Offset = 3.4f,
            Scale = 1f,
            Color = Color.White,
            Alpha = 0,
        };
    }

    public void ApplyAmmo(BaseAmmo baseAmmo)
    {
        DustData.Color = baseAmmo.Color;
    }

    public void CreateDust(Vector2 position, Vector2 velocity)
    {
        var offset = new Vector2(velocity.X, velocity.Y);
        offset.Normalize();
        offset *= DustData.Offset;

        for (int i = 0; i < DustData.Amount; i++)
        {
            var newPosition = new Vector2(position.X + offset.X * i, position.Y + offset.Y * i);
            var dust = Dust.NewDustPerfect(newPosition, DustID.Wet, new Vector2(0, 0), DustData.Alpha, DustData.Color, DustData.Scale);
            dust.noGravity = true;
            dust.fadeIn = 0f;
            dust.velocity = velocity.SafeNormalize(Vector2.Zero);
        }
    }

    public void DrawChain(Vector2 from, Vector2 to)
    {
        var texture = ModContent.Request<Texture2D>(ChainData.TexturePath);
        var source = ChainData.Source;
        var segmentLength = source.Height;

        var origin = source.Size() / 2f;
        var direction = to - from;
        var unit = direction.SafeNormalize(Vector2.Zero);

        var rotation = unit.ToRotation() + MathHelper.PiOver2;
        var drawLength = direction.Length() + segmentLength / 2f;

        while (drawLength > 0f)
        {
            var color = Lighting.GetColor(from.ToTileCoordinates());

            Main.spriteBatch.Draw(texture.Value, from - Main.screenPosition, source, color, rotation, origin, 1f, SpriteEffects.None, 0f);

            from += unit * segmentLength;
            drawLength -= segmentLength;
        }
    }
}