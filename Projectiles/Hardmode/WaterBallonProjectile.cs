using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Audio;

namespace WaterGuns.Projectiles.Hardmode
{
    public class WaterExplosion : BaseProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            AIType = ProjectileID.WaterGun;
            Projectile.damage = 1;
            Projectile.scale *= 4;
            Projectile.timeLeft = 12;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
        }
    }

    public class WaterBallonProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            AIType = ProjectileID.Grenade;
            Projectile.timeLeft = 600;
            base.affectedByHoming = false;
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item85);

            Projectile.NewProjectileDirect(data, Projectile.position, Vector2.Zero, ModContent.ProjectileType<WaterExplosion>(), Projectile.damage, Projectile.knockBack, Projectile.owner);

            for (int i = 0; i < 14; i++)
            {
                Vector2 speed = Main.rand.NextVector2Unit((float)MathHelper.Pi, (float)MathHelper.Pi) * Main.rand.NextFloat();
                speed *= 3;
                var dust = Dust.NewDustDirect(Projectile.position, 30, 30, DustID.Wet, speed.X, speed.Y, 75, data.color, 1.4f);
            }

            base.Kill(timeLeft);
        }

        protected float gravity = 0.04f;
        public override void AI()
        {
            Projectile.velocity.Y += gravity;
            Projectile.rotation += MathHelper.ToRadians(10);
            Projectile.velocity *= 0.997f;

            base.AI();
        }
    }
}
