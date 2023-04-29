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
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            Main.projFrames[Projectile.type] = 8;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Projectile.width = 58;
            Projectile.height = 62;
            Projectile.timeLeft = 140;

            Projectile.penetrate = -1;
            Projectile.tileCollide = false;

            Projectile.friendly = true;
            Projectile.hostile = false;
        }

        public NPC FindNearestNPC()
        {
            float nearestDist = -1;
            NPC nearestNpc = null;
            float detectRange = MathF.Pow(340f, 2);

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

        NPC target = null;
        int delay = 10;
        float speed = 1f;
        public override void AI()
        {
            base.AI();

            if (target == null)
            {
                target = FindNearestNPC();
            }

            if (target != null)
            {
                delay += 1;
                if (Projectile.Center.DistanceSQ(target.Center) < 32 * 32)
                {
                    if (delay >= 24 || speed != 1f)
                    {
                        delay = 0;
                        Projectile.velocity = Projectile.DirectionTo(target.Center).SafeNormalize(Vector2.Zero) * 4;
                    }

                    speed = 1f;
                }
                else
                {
                    if (delay >= 5)
                    {
                        delay = 0;
                        speed *= 1.05f;
                        Projectile.velocity = Projectile.DirectionTo(target.Center).SafeNormalize(Vector2.Zero) * 12 * speed;
                    }
                }

                if (target.GetLifePercent() <= 0f)
                {
                    target = null;
                }
            }


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
                velocity *= 14;
                Projectile.velocity = velocity;
            }

        }
    }
}
