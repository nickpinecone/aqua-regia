using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Terraria.DataStructures;
using System.Collections.Generic;

namespace WaterGuns.Projectiles
{
    public abstract class CommonWaterProjectile : ModProjectile
    {
        public WaterGuns.ProjectileData data = null;
        protected bool affectedByAmmoBuff = true;

        public int defaultTime = 0;
        public float defaultGravity = 0;
        protected float gravity = 0.001f;

        public float GetAngle(Vector2 u, Vector2 v)
        {
            var dot = u.X * v.X + u.Y * v.Y;
            var det = u.X * v.Y - u.Y * v.X;

            return MathF.Atan2(det, dot);
        }

        public NPC FindNearestNPC(float radius)
        {
            float nearestDist = -1;
            NPC nearestNpc = null;
            float detectRange = MathF.Pow(radius, 2);

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

        public override void SetDefaults()
        {
            // If derivatives dont call base.SetDefaults() they use Projectile.CloneDefaults(ProjectileID.WaterGun);
            // These are for creating custom projectiles resembling water gun projectile
            AIType = ProjectileID.WaterGun;

            Projectile.damage = 1;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 62;

            Projectile.width = 16;
            Projectile.height = 16;

            // Whithout extra updates it feels slow
            Projectile.extraUpdates = 2;

            Projectile.friendly = true;
            Projectile.hostile = false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            if (source is WaterGuns.ProjectileData newData)
            {
                data = newData;
            }
            else
            {
                data = new WaterGuns.ProjectileData(source);
            }

            defaultTime = Projectile.timeLeft;
            defaultGravity = gravity;

            if (Projectile.penetrate != -1 && Projectile.penetrate < 3 && data.penetrates)
            {
                Projectile.penetrate = 3;
            }

            base.OnSpawn(source);
        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);

            // Lava ammo effect
            if (data.buffType == BuffID.OnFire && affectedByAmmoBuff)
            {

                for (int i = 0; i < Main.rand.Next(1, 3); i++)
                {
                    var velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(0.8f, 1.2f);
                    velocity *= 8;

                    Projectile.NewProjectile(Projectile.GetSource_NaturalSpawn(), Projectile.Center, velocity, ModContent.ProjectileType<Projectiles.Ammo.LavaAmmoProjectile>(), Projectile.damage / 8, 0, Projectile.owner);
                }
            }

            // Venom ammo effect
            if (data.buffType == BuffID.Venom && affectedByAmmoBuff)
            {
                for (int i = 0; i < Main.rand.Next(2, 4); i++)
                {
                    var velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(0.8f, 1.2f);
                    velocity *= 12;

                    var proj = Projectile.NewProjectileDirect(Projectile.GetSource_NaturalSpawn(), Projectile.Center, velocity, ModContent.ProjectileType<Projectiles.Ammo.VenomAmmoProjectile>(), Projectile.damage / 4, 0, Projectile.owner);
                }
            }
        }

        int counter = 0;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (data.bounces)
            {
                if (counter < 3)
                {
                    counter += 1;

                    // Bounce off the wall without creating a new projectile
                    if (oldVelocity.X != Projectile.velocity.X) Projectile.velocity.X = -oldVelocity.X;
                    if (oldVelocity.Y != Projectile.velocity.Y) Projectile.velocity.Y = -oldVelocity.Y;

                    // Reset gravity and timeLeft so it doesnt destroy
                    Projectile.timeLeft = defaultTime;
                    gravity = defaultGravity;
                    return false;
                }
            }
            return base.OnTileCollide(oldVelocity);
        }

        Dictionary<NPC, int> immunityFrames = new Dictionary<NPC, int>();

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            immunityFrames[target] = 10;

            if (data.hasBuff && affectedByAmmoBuff)
            {
                target.AddBuff(data.buffType, data.buffTime);
            }
            if (data.spawnsStar)
            {
                int rotation = Main.rand.Next(-30, 0);
                var position = target.Center + new Vector2(Main.screenHeight, 0).RotatedBy(MathHelper.ToRadians(-75)).RotatedBy(MathHelper.ToRadians(rotation));
                var velocity = new Vector2(20, 0).RotatedBy(MathHelper.ToRadians(rotation - 180 - 75));
                Projectile.NewProjectile(Projectile.GetSource_NaturalSpawn(), position, velocity, ProjectileID.HallowStar, damage / 3, knockback, Main.myPlayer);
            }
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (immunityFrames.ContainsKey(target))
            {
                if (immunityFrames[target] == 0)
                {
                    return base.CanHitNPC(target);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                immunityFrames.Add(target, 0);
                return base.CanHitNPC(target);
            }
        }

        public void CreateDust()
        {
            // Dust creation resembling the in-game water gun projectile
            var offset = new Vector2(Projectile.velocity.X, Projectile.velocity.Y);
            offset.Normalize();
            offset *= 3.4f;

            for (int i = 0; i < data.dustAmount; i++)
            {
                var position = new Vector2(Projectile.Center.X + offset.X * i, Projectile.Center.Y + offset.Y * i);
                var dust = Dust.NewDustPerfect(position, DustID.Wet, new Vector2(0, 0), data.alpha, data.color, data.dustScale);
                dust.noGravity = true;
                dust.fadeIn = 0f;
                dust.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero);
            }
        }

        public void UpdateImmunityFrames()
        {
            foreach (var item in immunityFrames)
            {
                immunityFrames[item.Key] = Math.Max(0, item.Value - 1);
            }
        }

        public override void AI()
        {
            UpdateImmunityFrames();
            base.AI();
        }

        public void AutoAim()
        {
            // Homing code, I don't know how it works, just took it from the internet
            float num132 = (float)Math.Sqrt((double)(Projectile.velocity.X * Projectile.velocity.X + Projectile.velocity.Y * Projectile.velocity.Y));
            float num133 = Projectile.localAI[0];
            if (num133 == 0f)
            {
                Projectile.localAI[0] = num132;
                num133 = num132;
            }
            float num134 = Projectile.position.X;
            float num135 = Projectile.position.Y;
            float num136 = 300f;
            bool flag3 = false;
            int num137 = 0;
            if (Projectile.ai[1] == 0f)
            {
                for (int num138 = 0; num138 < 200; num138++)
                {
                    if (Main.npc[num138].CanBeChasedBy(this, false) && (Projectile.ai[1] == 0f || Projectile.ai[1] == (float)(num138 + 1)))
                    {
                        float num139 = Main.npc[num138].position.X + (float)(Main.npc[num138].width / 2);
                        float num140 = Main.npc[num138].position.Y + (float)(Main.npc[num138].height / 2);
                        float num141 = Math.Abs(Projectile.position.X + (float)(Projectile.width / 2) - num139) + Math.Abs(Projectile.position.Y + (float)(Projectile.height / 2) - num140);
                        if (num141 < num136 && Collision.CanHit(new Vector2(Projectile.position.X + (float)(Projectile.width / 2), Projectile.position.Y + (float)(Projectile.height / 2)), 1, 1, Main.npc[num138].position, Main.npc[num138].width, Main.npc[num138].height))
                        {
                            num136 = num141;
                            num134 = num139;
                            num135 = num140;
                            flag3 = true;
                            num137 = num138;
                        }
                    }
                }
                if (flag3)
                {
                    Projectile.ai[1] = (float)(num137 + 1);
                }
                flag3 = false;
            }
            if (Projectile.ai[1] > 0f)
            {
                int num142 = (int)(Projectile.ai[1] - 1f);
                if (Main.npc[num142].active && Main.npc[num142].CanBeChasedBy(this, true) && !Main.npc[num142].dontTakeDamage)
                {
                    float num143 = Main.npc[num142].position.X + (float)(Main.npc[num142].width / 2);
                    float num144 = Main.npc[num142].position.Y + (float)(Main.npc[num142].height / 2);
                    if (Math.Abs(Projectile.position.X + (float)(Projectile.width / 2) - num143) + Math.Abs(Projectile.position.Y + (float)(Projectile.height / 2) - num144) < 1000f)
                    {
                        flag3 = true;
                        num134 = Main.npc[num142].position.X + (float)(Main.npc[num142].width / 2);
                        num135 = Main.npc[num142].position.Y + (float)(Main.npc[num142].height / 2);
                    }
                }
                else
                {
                    Projectile.ai[1] = 0f;
                }
            }
            if (!Projectile.friendly)
            {
                flag3 = false;
            }
            if (flag3)
            {
                float num145 = num133;
                Vector2 vector10 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
                float num146 = num134 - vector10.X;
                float num147 = num135 - vector10.Y;
                float num148 = (float)Math.Sqrt((double)(num146 * num146 + num147 * num147));
                num148 = num145 / num148;
                num146 *= num148;
                num147 *= num148;
                int num149 = 8;
                Projectile.velocity.X = (Projectile.velocity.X * (float)(num149 - 1) + num146) / (float)num149;
                Projectile.velocity.Y = (Projectile.velocity.Y * (float)(num149 - 1) + num147) / (float)num149;
            }
        }
    }
}
