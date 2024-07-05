using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.Modules;

// TODO Incomplete

public class ChainModule : BaseProjectileModule
{
    public string TexturePath { get; set; }
    public Rectangle Source { get; set; }

    public ChainModule(BaseProjectile baseProjectile) : base(baseProjectile)
    {
    }

    public void DrawChain(Vector2 from, Vector2 to)
    {
        var texture = ModContent.Request<Texture2D>(TexturePath);
        var source = Source;
        var segmentLength = source.Height;

        var origin = source.Size() / 2f;
        var direction = to - from;
        var unitDirection = direction.SafeNormalize(Vector2.Zero);

        var rotation = unitDirection.ToRotation() + MathHelper.PiOver2;
        var drawLength = direction.Length() + segmentLength / 2f;

        while (drawLength > 0f)
        {
            var color = Lighting.GetColor(from.ToTileCoordinates());

            Main.spriteBatch.Draw(texture.Value, from - Main.screenPosition, source, color, rotation, origin, 1f,
                                  SpriteEffects.None, 0f);

            from += unitDirection * segmentLength;
            drawLength -= segmentLength;
        }
    }
}
