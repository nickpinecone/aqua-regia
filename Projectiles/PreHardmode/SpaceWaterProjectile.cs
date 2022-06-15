using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.PreHardmode
{
    public class SpaceWaterProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            AIType = ProjectileID.WaterGun;

            // Making my own projectile inspired by water gun projectile
            Projectile.damage = 1;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 62;

            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.extraUpdates = 2;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.penetrate = 2;
        }

        int counter = 0;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (counter < 4)
            {
                counter += 1;

                var velocity = -oldVelocity.RotatedByRandom(MathHelper.ToRadians(45));
                Projectile.velocity = velocity;
                Projectile.timeLeft = 62;
                gravity = 0.001f;
                return false;
            }
            return base.OnTileCollide(oldVelocity);
        }

        public override void Kill(int timeLeft)
        {
            // Spawn dust when projectile dies, copying water gun behavior
            for (int i = -5; i < 0; i++)
            {
                Projectile.velocity.Normalize();
                Projectile.velocity *= 4;
                var dust = Dust.NewDust(new Vector2(Projectile.position.X - 20, Projectile.position.Y - 5), 60, 10, 10, Projectile.velocity.X, Projectile.velocity.Y, 0, new Color(61, 192, 194), 0.6f);

                base.Kill(timeLeft);
            }
        }

        float gravity = 0.001f;
        public override void AI()
        {
            // Curve it like the in-game water gun projectile
            gravity += 0.002f;
            Projectile.velocity.Y += gravity;

            // Creating some dust to see the projectile
            var dust = Dust.NewDustPerfect(Projectile.Center, 211, new Vector2(0, 0), 0, new Color(61, 192, 194), 1.5f);
            dust.fadeIn = 1;
            dust.noGravity = true;

            base.AI();
        }
    }
}
