using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.Hardmode
{

    public class Waternado : ModProjectile
    {
        public float GetAngle(Vector2 u, Vector2 v)
        {
            var dot = u.X * v.X + u.Y * v.Y;
            var det = u.X * v.Y - u.Y * v.X;

            return MathF.Atan2(det, dot);
        }

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            Main.projFrames[Projectile.type] = 8;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

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

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);

            for (int i = 0; i < 10; i++)
            {
                Vector2 speed = Main.rand.NextVector2Unit();

                var position = Projectile.Center;
                var dust = Dust.NewDustDirect(position, 16, 16, DustID.Wet, 0, 0, 0, new Color(79, 116, 199), 1f);
                dust.noGravity = true;
                dust.velocity = speed * 8;
            }
        }

        public NPC FindNearestNPC()
        {
            float nearestDist = -1;
            NPC nearestNpc = null;
            float detectRange = MathF.Pow(380f, 2);

            for (int i = 0; i < Main.npc.Length; i++)
            {
                NPC target = Main.npc[i];

                if (!target.CanBeChasedBy())
                {
                    continue;
                }

                var dist = Projectile.Center.DistanceSQ(target.Center);

                if (dist <= detectRange && (dist < nearestDist || nearestDist == -1))
                {
                    nearestDist = dist;
                    nearestNpc = target;
                }
            }

            return nearestNpc;
        }


        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
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


            target = FindNearestNPC();

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

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            // target.buffImmune[ModContent.BuffType<Buffs.BubbleWhirlDebuff>()] = false;
            // target.AddBuff(ModContent.BuffType<Buffs.BubbleWhirlDebuff>(), 60 * 3);
            base.OnHitNPC(target, damage, knockback, crit);
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
