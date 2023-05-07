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
            base.Kill(timeLeft);

            for (int i = 0; i < 10; i++)
            {
                Vector2 speed = Main.rand.NextVector2Unit();

                var position = Projectile.Center;
                var dust = Dust.NewDustPerfect(position, DustID.Wet, new Vector2(0, 0), 75, default, 1f);
                dust.noGravity = true;
                dust.velocity = speed * 4;
            }

            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item54);
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
            Projectile.width = 20;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
            Projectile.damage = 1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 80;
            Projectile.scale = 0.8f;
        }

        Vector2 hitPoint = Vector2.Zero;
        NPC hitTarget = null;
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
            if (hitPoint == Vector2.Zero)
            {
                var dir = Projectile.position.DirectionTo(target.position);
                var dist = Projectile.position.Distance(target.position);

                hitPoint = dir * dist;
                hitTarget = target;

                Projectile.velocity = Vector2.Zero;
                applyGravity = false;
            }
        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);

            for (int i = 0; i < 5; i++)
            {
                Vector2 speed = Main.rand.NextVector2Unit();

                var position = Projectile.Center;
                var dust = Dust.NewDustDirect(position, 8, 8, DustID.Coralstone, 0, 0, 0, default, 0.9f);
                dust.noGravity = true;
                dust.velocity = speed * 4;
            }
        }

        protected float gravity = 0.005f;
        protected bool applyGravity = true;
        public override void AI()
        {
            if (applyGravity)
            {
                gravity += 0.005f;
                Projectile.velocity.Y += gravity;
            }

            if (Projectile.velocity.X > 0)
            {
                Projectile.rotation += 0.16f;
            }
            else if (Projectile.velocity.X < 0)
            {
                Projectile.rotation -= 0.16f;
            }

            if (hitTarget != null)
            {
                Projectile.position = hitTarget.position - hitPoint;

                if (hitTarget.GetLifePercent() < 0f)
                {

                    Projectile.Kill();
                }
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
                offset.Y = target.Bottom.Y - Main.rand.Next(5, 10);

                // Dont know how to extract IEventSource from BubbleProjectile so using OceanWaterProjectile source
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), offset, new Vector2(0, -4), ModContent.ProjectileType<BubbleProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
            base.OnHitNPC(target, damage, knockback, crit);
        }
    }
}
