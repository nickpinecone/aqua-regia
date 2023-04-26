using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.Hardmode
{
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
            target.AddBuff(ModContent.BuffType<Buffs.BubbleWhirlDebuff>(), 60 * 3);
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
