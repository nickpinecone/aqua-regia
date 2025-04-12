using System;
using WaterGuns.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Modules.Projectiles;

public class LaserModule : IModule
{
    public float MaxDistance { get; set; } = 2200f;
    public Texture2D? Texture { get; private set; }

    private Vector2 _start = Vector2.Zero;
    private Vector2 _direction = Vector2.Zero;
    private float _distance = 0f;

    public void SetDefaults(BaseProjectile baseProjectile)
    {
        baseProjectile.Projectile.width = 10;
        baseProjectile.Projectile.height = 10;
        baseProjectile.Projectile.penetrate = -1;

        baseProjectile.Projectile.hostile = false;
        baseProjectile.Projectile.friendly = true;
        baseProjectile.Projectile.tileCollide = false;

        baseProjectile.Projectile.damage = 0;
        baseProjectile.Projectile.knockBack = 0;
        baseProjectile.Projectile.timeLeft = 0;
    }

    public void SetTexture(string path)
    {
        Texture = ModContent.Request<Texture2D>(path).Value;
    }

    public bool? Colliding(Rectangle projectileHitbox, Rectangle targetHitbox)
    {
        float point = 0f;
        return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), _start,
                                                 _start + _direction * _distance, 22, ref point);
    }

    public void Update(Vector2 start, Vector2 direction)
    {
        direction.Normalize();

        _start = start;
        _direction = direction;

        for (_distance = 0; _distance <= MaxDistance; _distance += 5f)
        {
            var point = start + direction * _distance;

            var tile = TileHelper.GetTile(point);
            if (TileHelper.IsSolid(point))
            {
                _distance -= 5f;
                break;
            }
        }
    }

    public void DrawLaser()
    {
        float rotation = _direction.ToRotation() + -MathHelper.PiOver2;

        for (float i = 0; i <= _distance; i += 10)
        {
            Color color = Color.White;
            var origin = _start + i * _direction;
            Main.spriteBatch.Draw(Texture, origin - Main.screenPosition, new Rectangle(0, 26, 28, 26), color, rotation,
                                  new Vector2(14, 13), 1f, 0, 0);
        }

        Main.spriteBatch.Draw(Texture, _start + _direction * -10f - Main.screenPosition, new Rectangle(0, 0, 28, 26),
                              Color.White, rotation, new Vector2(28 * .5f, 26 * .5f), 1f, 0, 0);

        Main.spriteBatch.Draw(Texture, _start + (_distance + 10f) * _direction - Main.screenPosition,
                              new Rectangle(0, 52, 28, 26), Color.White, rotation, new Vector2(28 * .5f, 26 * .5f), 1f,
                              0, 0);

        SpawnDusts();
    }

    private void SpawnDusts()
    {
        var vector = _direction.RotatedBy(MathHelper.Pi);
        var start = vector.RotatedBy(-MathHelper.PiOver4);
        var end = vector.RotatedBy(MathHelper.PiOver4);

        var position = _start + _direction * (_distance + 15f);

        foreach (var particle in Particle.ArcPerfect(DustID.Electric, position, start, end, 4, 3f, 1f))
        {
            particle.velocity = particle.velocity.RotatedByRandom(0.4f);
            particle.velocity *= Main.rand.NextFloat(0.8f, 1.4f);
            particle.noGravity = true;
        }
    }
}
