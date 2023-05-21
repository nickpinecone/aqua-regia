using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.Hardmode.MechanicalWaterProjectiles
{
    public class WaterGrenade : ModProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 14;
            Projectile.height = 20;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.tileCollide = true;
        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);

            SoundEngine.PlaySound(SoundID.Item14);

            Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<WaterExplosion>(), Projectile.damage, Projectile.knockBack, Projectile.owner);

            for (int i = 0; i < 8; i++)
            {
                var rotation = Main.rand.Next(0, 360);
                var velocity = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(rotation));
                velocity *= 4f;
                var dust = Dust.NewDustDirect(Projectile.Center, 8, 8, DustID.Cloud, velocity.X, velocity.Y, 75, default);
                dust.scale = 3;
                dust.noGravity = true;
            }
        }

        float gravity = 0.02f;
        public override void AI()
        {
            base.AI();

            Projectile.velocity.Y += gravity;
            gravity += 0.005f;

            if (Projectile.velocity.X > 0)
            {
                Projectile.rotation += 0.16f;
            }
            else if (Projectile.velocity.X < 0)
            {
                Projectile.rotation -= 0.16f;
            }
        }

    }

    public class LauncherProjectile : MechanicalProjectile
    {
        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);

            delay = 60;
            delayMax = 60;

            dist = -(Main.player[Main.myPlayer].Center - Projectile.Center);
            data.fullCharge = false;
            projType = ModContent.ProjectileType<WaterGrenade>();

            mouseLeftNeed = false;
            Projectile.timeLeft = 210;
        }
    }
}
