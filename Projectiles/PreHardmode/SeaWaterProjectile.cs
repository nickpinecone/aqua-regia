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

    public class StarfishProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
            Projectile.damage = 1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 120;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            // Bounce off the wall without creating a new projectile
            if (oldVelocity.X != Projectile.velocity.X) Projectile.velocity.X = -oldVelocity.X;
            if (oldVelocity.Y != Projectile.velocity.Y) Projectile.velocity.Y = -oldVelocity.Y;

            return false;
        }

        protected float gravity = 0.004f;
        public override void AI()
        {
            Projectile.scale = 1.6f;
            gravity += 0.006f;
            Projectile.velocity.Y += gravity;

            if (Projectile.velocity.X > 0)
            {
                Projectile.rotation += 0.16f;
            }
            else
            {
                Projectile.rotation -= 0.16f;
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
