using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.Hardmode
{

    public class Waternado : BaseProjectile
    {

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 8;
        }

        public override void SetDefaults()
        {
            AIType = ProjectileID.WeatherPainShot;

            Projectile.width = 58;
            Projectile.height = 62;
            Projectile.timeLeft = 140;
            Projectile.light = 1f;

            Projectile.penetrate = -1;
            Projectile.tileCollide = false;

            Projectile.friendly = true;
            Projectile.hostile = false;
        }

        public override void OnKill(int timeLeft)
        {
            base.OnKill(timeLeft);

            for (int i = 0; i < 10; i++)
            {
                Vector2 speed = Main.rand.NextVector2Unit();

                var position = Projectile.Center;
                var dust = Dust.NewDustDirect(position, 16, 16, DustID.Wet, 0, 0, 0, new Color(79, 116, 199), 1f);
                dust.noGravity = true;
                dust.velocity = speed * 8;
            }
        }

        NPC target = null;
        int delay = 10;
        float speed = 1f;
        float curve = 0.1f;
        public override void AI()
        {
            base.AI();

            // Cool dust trail effect
            for (int i = -2; i < 2; i++)
            {
                var vel = Projectile.velocity.SafeNormalize(Vector2.Zero);

                var dust = Dust.NewDust(Projectile.Center, 12, 12, DustID.Wet, -vel.X * 5, -vel.Y * 5, Main.rand.Next(125, 175), new Color(79, 116, 199), 1.1f);
                Main.dust[dust].position = Projectile.Center + vel.RotatedBy(MathHelper.PiOver2 * MathF.Sign(i)) * MathF.Abs(i) * 12;
                Main.dust[dust].noGravity = true;
            }


            target = FindNearestNPC(380f);

            if (target != null)
            {
                var dir = target.Center - Projectile.Center;
                var vel = Projectile.velocity;

                var ang = GetAngle(vel, dir);

                Projectile.velocity = Projectile.velocity.RotatedBy(MathF.Sign(ang) * MathF.Min(curve, MathF.Abs(ang)));
                Projectile.velocity.Normalize();
                Projectile.velocity *= speed * 16;

                if (Projectile.Center.DistanceSQ(target.Center) < 32 * 32)
                {
                    curve = 0.3f;
                }

                else
                {
                    curve *= 1.01f;
                }
            }

            speed = 1f;


            if (++Projectile.frameCounter >= 6)
            {
                Projectile.frameCounter = 0;
                // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }
        }
    }

    public class OceanWaterProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            AIType = ProjectileID.WaterGun;
            Projectile.tileCollide = false;
            Projectile.hostile = false;
        }

        Vector2 dest = Vector2.Zero;
        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
            dest = new Vector2(0, 128).RotatedByRandom(MathHelper.ToRadians(360));
            Projectile.position = Main.player[Main.myPlayer].Center;
            Projectile.velocity = Vector2.Zero;

            data.dustScale = 1;
        }

        int inactiveTime = 60;
        int time = 0;
        bool launched = false;
        public override void AI()
        {
            base.AI();
            base.CreateDust();

            time++;
            if (time < inactiveTime)
            {
                if ((Projectile.Center - Main.player[Main.myPlayer].Center).Length() < 128)
                {
                    var velocity = (Main.player[Main.myPlayer].Center + dest) - Projectile.Center;
                    velocity.Normalize();
                    velocity *= 5;
                    Projectile.velocity = velocity;
                }
                else
                {
                    Projectile.velocity = Vector2.Zero;
                }

                Projectile.timeLeft = 120;
            }
            else if (!launched)
            {
                launched = true;
                Projectile.friendly = true;
                var velocity = Main.MouseWorld - Projectile.Center;
                velocity.Normalize();
                velocity *= 12;
                Projectile.velocity = velocity;
            }

        }
    }
}
