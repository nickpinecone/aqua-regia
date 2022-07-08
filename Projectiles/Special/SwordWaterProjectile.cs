using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.Special
{
    public class SwordWaterProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            // If derivatives dont call base.SetDefaults() they use Projectile.CloneDefaults(ProjectileID.WaterGun);
            // These are for creating custom projectiles resembling water gun projectile
            AIType = ProjectileID.WaterGun;

            Projectile.damage = 1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 24;

            Projectile.width = 16;
            Projectile.height = 16;

            // Whithout extra updates it feels slow
            Projectile.extraUpdates = 2;

            Projectile.friendly = true;
            Projectile.hostile = false;
        }

        public void CreateDust(Color color, float scale)
        {
            // Dust creation resembling the in-game water gun projectile
            var offset = new Vector2(Projectile.velocity.X, Projectile.velocity.Y);
            offset.Normalize();
            offset *= 3;

            for (int i = 0; i < 4; i++)
            {
                var position = new Vector2(Projectile.Center.X + offset.X * i, Projectile.Center.Y + offset.Y * i);
                var dust = Dust.NewDustPerfect(position, DustID.Wet, new Vector2(0, 0), 0, color, scale);
                dust.noGravity = true;
                dust.fadeIn = 0f;
            }
        }

        Vector2 offset = new Vector2(0, 1);
        public override void AI()
        {
            CreateDust(default, 1);
            Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(0.4f));
            base.AI();
        }
    }
}
