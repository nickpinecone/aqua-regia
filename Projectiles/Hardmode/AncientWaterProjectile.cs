using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace WaterGuns.Projectiles.Hardmode
{
    public class EarthBoulder : ModProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Projectile.width = 32;
            Projectile.height = 32;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 300;
        }

        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);

            offset = Main.rand.Next(-3, 3);
        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);

            SoundEngine.PlaySound(SoundID.Item14);

            for (int i = 0; i < 6; i++)
            {
                var rotation = Main.rand.Next(0, 360);
                var velocity = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(rotation));
                velocity *= 4f;
                var dust = Dust.NewDustDirect(Projectile.Center, 8, 8, DustID.Cloud, velocity.X, velocity.Y, 75, default);
                dust.scale = 2;
                dust.noGravity = true;
                dust.color = new Color(233, 132, 56);

            }
        }

        int delay = 0;
        int dir = Main.rand.NextFromList(new int[] { -1, 1 });
        float dir2 = 0f;
        int offset = 0;
        float gravity = 0.1f;
        float speed = 0.3f;
        public Projectile sandstorm = null;
        bool aroundAnim = Main.rand.NextBool();
        bool inSandstorm = false;

        public override void AI()
        {
            if (Projectile.timeLeft >= 100)
            {
                delay += dir;
                if (delay >= 10 + offset)
                {
                    dir = -dir;
                }
                else if (delay <= -10 + offset)
                {
                    dir = -dir;
                }

                if (Main.rand.Next(0, 10) == 1)
                {
                    dir2 = Main.rand.NextFromList(new float[] { -0.2f, 0.2f });
                }

                if (Projectile.Center.Y < sandstorm.Top.Y || Projectile.Center.Y > sandstorm.Bottom.Y)
                {
                    dir2 = -dir2;
                }

                if (Projectile.Center.X > sandstorm.Left.X && Projectile.Center.X < sandstorm.Right.X)
                {
                    inSandstorm = true;

                    if (aroundAnim)
                    {
                        Projectile.alpha = 55;
                    }
                }
                else
                {
                    if (inSandstorm)
                    {
                        inSandstorm = false;
                        aroundAnim = !aroundAnim;
                        Projectile.alpha = 0;
                    }
                }

                speed *= 1.01f;

                Projectile.velocity = new Vector2(dir, dir2) * Main.rand.Next(4, 10) * speed;
            }
            else
            {
                Projectile.alpha = 0;
                Projectile.velocity.Y += gravity;
                gravity += 0.05f;
            }

        }
    }

    public class BoulderSandstorm : ModProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
        }

        Projectile sandstorm;
        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);

            sandstorm = Projectile.NewProjectileDirect(source, Projectile.Center, Vector2.Zero, ProjectileID.SandnadoFriendly, Projectile.damage, Projectile.knockBack, Projectile.owner);
            sandstorm.timeLeft = 200;
            Projectile.timeLeft = sandstorm.timeLeft;

        }

        int delay = 0;
        bool spawned = false;
        public override void AI()
        {
            base.AI();

            if (delay >= 10 && !spawned)
            {
                spawned = true;

                var start = sandstorm.Top + new Vector2(0, sandstorm.height / 8);
                for (int i = 0; i < 6; i++)
                {
                    var proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), start, Vector2.Zero, ModContent.ProjectileType<EarthBoulder>(), Projectile.damage, 0, Projectile.owner);
                    (proj.ModProjectile as EarthBoulder).sandstorm = sandstorm;
                    start += new Vector2(0, sandstorm.height / 6);
                }
            }
            else
            {
                delay += 1;
            }
        }

    }

    public class GeyserPlatform : ModProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Projectile.width = 96;
            Projectile.height = 48;
            Projectile.timeLeft = 60;
        }
    }

    public class AncientGeyser : ModProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Projectile.width = 64;
            Projectile.height = 448;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 60;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire3, 60);
            base.OnHitNPC(target, hit, damageDone);
        }

        public override void AI()
        {
            base.AI();
            if (Projectile.ai[0] == 1)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Bottom + new Vector2(0, 24), Vector2.Zero, ModContent.ProjectileType<GeyserPlatform>(), 0, 0, Projectile.owner);
                Projectile.ai[0] = 0;
            }
        }
    }

    public class AncientWaterProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            AIType = ProjectileID.WaterGun;
        }

        public override void AI()
        {
            base.AI();
            base.CreateDust();
        }
    }
}
