using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace WaterGuns.Projectiles.PreHardmode
{
    public class SpaceWaterProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Projectile.penetrate = 2;
        }

        int counter = 0;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (counter < 4)
            {
                counter += 1;

                // Bounce off the wall without creating a new projectile
                var velocity = -oldVelocity.RotatedByRandom(MathHelper.ToRadians(45));
                Projectile.velocity = velocity;

                // Reset gravity and timeLeft so it doesnt destroy
                Projectile.timeLeft = 62;
                base.gravity = 0.001f;
                return false;
            }
            return base.OnTileCollide(oldVelocity);
        }

        public override void Kill(int timeLeft)
        {
            // Spawn dust when projectile dies, copying water gun behavior
            for (int i = 0; i < 14; i++)
            {
                var offset = new Vector2(Projectile.Center.X - MathF.Abs(Projectile.velocity.X * 48), Projectile.Center.Y - MathF.Abs(Projectile.velocity.Y * 48));

                Projectile.velocity.Normalize();
                var velocity = Projectile.velocity * 8;

                var dust = Dust.NewDust(offset, 60, 5, DustID.Wet, velocity.X, velocity.Y, 0, default, 0.6f);
                Main.dust[dust].fadeIn = 0.2f;

                base.Kill(timeLeft);
            }
        }

        public override void AI()
        {
            base.AI();

            // Creating some dust to see the projectile
            var offset = new Vector2(Projectile.velocity.X, Projectile.velocity.Y);
            offset.Normalize();
            offset *= 3;

            for (int i = 0; i < 4; i++)
            {
                var position = new Vector2(Projectile.Center.X + offset.X * i, Projectile.Center.Y + offset.Y * i);
                var dust = Dust.NewDustPerfect(position, DustID.Wet, new Vector2(0, 0));
                dust.noGravity = true;
                dust.fadeIn = 1;
            }
        }
    }
}
