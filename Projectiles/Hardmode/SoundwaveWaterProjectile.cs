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
        }

        bool isUp = true;
        public override void OnSpawn(IEntitySource source)
        {
            if (source is WaterGuns.ProjectileData data)
            {
                isUp = data.isUp;
            }
            base.OnSpawn(source);
        }

        public override void AI()
        {
            base.AI();
            base.CreateDust(default, 1.5f, 1, 1);

            int direction = isUp ? 1 : -1;
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
        }

        int delay = 10;
        int count = 1;
        public override void AI()
        {
            base.AI();

            if (delay > 5)
            {
                delay = 0;

                var velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(90));

                data.isUp = true;
                var projUp = Projectile.NewProjectileDirect(data, Projectile.position, velocity, ModContent.ProjectileType<SoundwaveProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                projUp.timeLeft += count * 3;

                velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(-90));

                data.isUp = false;
                var projDown = Projectile.NewProjectileDirect(data, Projectile.position, velocity, ModContent.ProjectileType<SoundwaveProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                projDown.timeLeft += count * 3;

                count += 1;
            }
            delay += 1;

        }
    }
}
