using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.Hardmode
{
    public class SoundwaveProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            AIType = ProjectileID.WaterGun;
            Projectile.timeLeft = 5;
            Projectile.penetrate = -1;
            Projectile.width = 90;

            base.affectedByHoming = false;
            base.affectedByBounce = false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);


            data.dustScale = 1.5f;
            data.dustAmount = 2;
            data.fadeIn = 1;
        }

        public override void AI()
        {
            base.AI();
            base.CreateDust();

            int direction = (int)Projectile.ai[0];
            Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(0.5f * direction));
        }
    }

    public class SoundwaveWaterProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            AIType = ProjectileID.WaterGun;
            Projectile.friendly = false;
            Projectile.extraUpdates = 1;
            base.affectedByHoming = false;
            Projectile.timeLeft -= 10;
        }

        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
        }

        int delay = 10;
        int count = 0;
        public override void AI()
        {
            base.AI();

            if (delay > 5)
            {
                delay = 0;

                var velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(90));

                var projUp = Projectile.NewProjectileDirect(data, Projectile.position, velocity, ModContent.ProjectileType<SoundwaveProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 1);
                projUp.timeLeft += count * 2;

                velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(-90));

                var projDown = Projectile.NewProjectileDirect(data, Projectile.position, velocity, ModContent.ProjectileType<SoundwaveProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner, -1);
                projDown.timeLeft += count * 2;

                count += 1;
            }
            delay += 1;
        }
    }
}
