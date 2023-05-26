using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using System;

namespace WaterGuns.Projectiles.Hardmode
{
    public class RocketWaterProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.penetrate = 1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            AIType = ProjectileID.RocketI;
            Projectile.damage = 1;
            Projectile.timeLeft = 120;
            Projectile.tileCollide = true;
            Projectile.alpha = 30;

            base.affectedByHoming = false;
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14);

            for (int i = 0; i < 6; i++)
            {
                var rotation = Main.rand.Next(0, 360);
                var velocity = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(rotation));
                velocity *= 6f;
                var dust = Dust.NewDustDirect(Projectile.Center, 8, 8, DustID.Cloud, velocity.X, velocity.Y, 75, default);
                dust.scale = 3;
                dust.noGravity = true;
            }

            for (int i = 0; i < 3; i++)
            {
                var velocity = new Vector2(10, 0).RotatedByRandom(MathHelper.ToRadians(180));
                data.homesIn = false;
                var proj = Projectile.NewProjectileDirect(base.data, Projectile.Center, velocity, ModContent.ProjectileType<WaterProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                proj.tileCollide = false;
                proj.penetrate = -1;
                proj.timeLeft -= 30;
            }

            base.Kill(timeLeft);
        }

        public override void AI()
        {
            base.AI();
            base.AutoAim(400f);

            Projectile.rotation = Projectile.velocity.ToRotation();

            // Terraria source code shenanigans
            if ((double)Math.Abs((float)Projectile.velocity.X) >= 8.0 || (double)Math.Abs((float)Projectile.velocity.Y) >= 8.0)
            {
                for (int index1 = 0; index1 < 2; ++index1)
                {
                    float num1 = 0.0f;
                    float num2 = 0.0f;
                    if (index1 == 1)
                    {
                        num1 = (float)(Projectile.velocity.X * 0.5);
                        num2 = (float)(Projectile.velocity.Y * 0.5);
                    }
                    if (Main.rand.Next(4) == 0)
                    {
                        int index3 = Dust.NewDust(Vector2.Subtract(new Vector2((float)(Projectile.position.X + 3.0) + num1, (float)(Projectile.position.Y + 3.0) + num2), Vector2.Multiply(Projectile.velocity, 0.5f)), Projectile.width - 8, Projectile.height - 8, 31, 0.0f, 0.0f, 100, new Color(107, 203, 255), 0.5f);
                        Main.dust[index3].fadeIn = (float)(1.0 + (double)Main.rand.Next(5) * 0.100000001490116);
                        Dust dust3 = Main.dust[index3];
                        dust3.velocity = Vector2.Multiply(dust3.velocity, 0.05f);
                    }
                }
            }
            if ((double)Math.Abs((float)Projectile.velocity.X) < 15.0 && (double)Math.Abs((float)Projectile.velocity.Y) < 15.0)
                Projectile.velocity = Vector2.Multiply(Projectile.velocity, 1.1f);
        }
    }
}
