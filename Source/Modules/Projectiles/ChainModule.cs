using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;

namespace AquaRegia.Modules.Projectiles;

public class ChainModule : BaseProjectileModule
{
    public Asset<Texture2D> Texture { get; private set; }
    public Rectangle Source { get; private set; }

    public float MaxDistance { get; set; }
    public float BackSpeed { get; set; }
    public Vector2 SpawnPosition { get; set; }
    public float PlayerClose { get; set; }

    public bool IsFarAway { get; private set; }
    public bool Returned { get; private set; }

    public ChainModule(BaseProjectile baseProjectile) : base(baseProjectile)
    {
    }

    public void SetDefaults(float maxDistance = 0f, float backSpeed = 0f, Vector2? spawnPosition = null,
                            float playerClose = 16f)
    {
        MaxDistance = maxDistance;
        BackSpeed = backSpeed;
        SpawnPosition = spawnPosition ?? Vector2.Zero;
        PlayerClose = playerClose;
    }

    public void SetTexture(string path, Rectangle rect)
    {
        Texture = ModContent.Request<Texture2D>(path);
        Source = rect;
    }

    public bool Update(Vector2 position)
    {
        if (!IsFarAway && position.DistanceSQ(SpawnPosition) > MaxDistance * MaxDistance)
        {
            IsFarAway = true;
        }

        return IsFarAway;
    }

    public Vector2 ReturnToPlayer(Vector2 returnTo, Vector2 position, Vector2 velocity, int slowFrames = 20)
    {
        var direction = returnTo - position;
        direction.Normalize();
        velocity = direction * BackSpeed;

        if (returnTo.DistanceSQ(position) < PlayerClose * PlayerClose)
        {
            Returned = true;
        }

        return velocity;
    }

    public void DrawChain(Vector2 from, Vector2 to)
    {
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

            Main.spriteBatch.Draw(Texture.Value, from - Main.screenPosition, source, color, rotation, origin, 1f,
                                  SpriteEffects.None, 0f);

            from += unitDirection * segmentLength;
            drawLength -= segmentLength;
        }
    }
}
