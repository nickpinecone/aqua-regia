using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.PreHardmode
{
    public class BubbleProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            AIType = ProjectileID.Bubble;

            Projectile.damage = 1;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 40;
            Projectile.tileCollide = true;

            Projectile.width = 16;
            Projectile.height = 16;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.alpha = 75;
        }

        public override void OnSpawn(IEntitySource source)
        {
            int random = Main.rand.Next(-5, 5);
            randomOffset1 = random;

            random = Main.rand.Next(-5, 5);
            randomOffset2 = random;

            base.OnSpawn(source);
        }

        public override void Kill(int timeLeft)
        {
            if (Projectile.scale == 4)
            {
                for (int i = 0; i < Main.rand.Next(2, 4); i++)
                {
                    // Offset randomly
                    var offset = new Vector2();
                    offset.X = Projectile.Center.X + Main.rand.Next(-50, 50);
                    offset.Y = Projectile.Center.Y + Main.rand.Next(-50, 50);

                    // Dont know how to extract IEventSource from BubbleProjectile so using OceanWaterProjectile source
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), offset, new Vector2(0, -4), ModContent.ProjectileType<BubbleProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                }
            }

            base.Kill(timeLeft);
        }

        int delay = 0;
        int randomOffset1 = 0;
        int randomOffset2 = 0;
        public override void AI()
        {
            delay += 1;
            if (delay < 20 + randomOffset1)
            {
                Projectile.position += new Vector2(1.5f, 0);
            }
            else if (delay < 40 + randomOffset2)
            {
                Projectile.position += new Vector2(-1.5f, 0);
            }
            else
            {
                delay = 0;
            }

            base.AI();
        }
    }

    public class BubbleWhirl : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 30;
        }

        public override void Kill(int timeLeft)
        {
            var proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(0, -4), ModContent.ProjectileType<BubbleProjectile>(), Projectile.damage / 4, Projectile.knockBack, Projectile.owner);
            proj.tileCollide = false;
            proj.penetrate = -1;
            proj.scale = 4;
            proj.timeLeft = 40;
            base.Kill(timeLeft);
        }

        int delay = 10;
        public override void AI()
        {
            delay += 1;
            if (delay > 10)
            {
                delay = 0;
                var offset = new Vector2();
                offset.X = Projectile.Bottom.X + Main.rand.Next(-60, 60);
                offset.Y = Projectile.Bottom.Y - Main.rand.Next(5, 40);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), offset, new Vector2(0, -4), ModContent.ProjectileType<BubbleProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
            base.AI();
        }
    }

    public class SeaWaterProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            AIType = ProjectileID.WaterGun;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (data.fullCharge)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Bottom, new Vector2(0, 0), ModContent.ProjectileType<BubbleWhirl>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
            else
            {
                // Offset randomly
                var offset = new Vector2();
                offset.X = target.Bottom.X + Main.rand.Next(-60, 60);
                offset.Y = target.Bottom.Y - Main.rand.Next(5, 40);

                // Dont know how to extract IEventSource from BubbleProjectile so using OceanWaterProjectile source
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), offset, new Vector2(0, -4), ModContent.ProjectileType<BubbleProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
            base.OnHitNPC(target, damage, knockback, crit);
        }
    }
}
