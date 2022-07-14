using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace WaterGuns.Projectiles.Hardmode
{
    public abstract class BaseProjectile : ModProjectile
    {
        public WaterGuns.ProjectileData data = null;

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
            base.OnSpawn(source);
        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (data.confusionBuff)
            {
                target.AddBuff(BuffID.Confused, 2);
            }
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public void CreateDust(Color color = default, float scale = 1.2f, int amount = 4, float fadeIn = 1, int alpha = 75)
        {
            if (data.color != default)
            {
                color = data.color;
            }
            else if (data.alpha != 75)
            {
                alpha = data.alpha;
            }
            else if (data.fadeIn != 1)
            {
                fadeIn = data.fadeIn;
            }
            else if (data.dustScale != 1.2f)
            {
                scale = data.dustScale;
            }
            else if (data.dustAmount != 4)
            {
                amount = data.dustAmount;
            }

            // Dust creation resembling the in-game water gun projectile
            var offset = new Vector2(Projectile.velocity.X, Projectile.velocity.Y);
            offset.Normalize();
            offset *= 3;

            for (int i = 0; i < amount; i++)
            {
                var position = new Vector2(Projectile.Center.X + offset.X * i, Projectile.Center.Y + offset.Y * i);
                var dust = Dust.NewDustPerfect(position, DustID.Wet, new Vector2(0, 0), data.alpha, color, scale);
                dust.noGravity = true;
                dust.fadeIn = fadeIn;
            }
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
