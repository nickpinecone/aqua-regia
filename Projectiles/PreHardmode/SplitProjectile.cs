using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.PreHardmode
{
    public class SplitProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            AIType = ProjectileID.WaterGun;
            Projectile.timeLeft = 24;
        }

        public override void Kill(int timeLeft)
        {
            if (timeLeft > 0)
            {
                return;
            }

            if (data.splitCount < 3)
            {
                data.splitCount += 1;

                for (int i = 0; i < 12; i++)
                {
                    var rotation = Main.rand.Next(0, 360);
                    var velocity = new Vector2(0, -1.4f).RotatedBy(MathHelper.ToRadians(rotation));
                    velocity *= 3f;
                    var dust = Dust.NewDustDirect(Projectile.Center, 8, 8, DustID.Cloud, velocity.X, velocity.Y, 75, default);
                    dust.scale = 2f;
                    dust.noGravity = true;
                }

                for (int i = -1; i < 2; i += 2)
                {
                    int distanceBetween = 8;
                    Vector2 modifiedVelocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(distanceBetween * i));
                    Projectile.NewProjectile(data, Projectile.Center, modifiedVelocity, Projectile.type, Projectile.damage, Projectile.knockBack, Projectile.owner);
                }
            }
            base.Kill(timeLeft);
        }
    }
}
