using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.Hardmode
{
    public class HarpyFeather : ModProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
        }

        public override void AI()
        {
            base.AI();

            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public override void OnKill(int timeLeft)
        {
            base.OnKill(timeLeft);

            for (int i = 0; i < 4; i++)
            {
                Vector2 speed = Main.rand.NextVector2Unit();

                var position = Projectile.Center;
                var dust = Dust.NewDustDirect(position, 8, 8, DustID.Harpy, 0, 0, 0, default, 0.9f);
                dust.noGravity = true;
                dust.velocity = speed * 4;
            }
        }
    }


    public class DamageZone : ModProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Projectile.width = 128;
            Projectile.height = 128;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 60;
        }
    }

    public class TitaniumWaterProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            AIType = ProjectileID.WaterGun;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
            if (data.fullCharge)
            {
                for (int i = -4; i < 4; i++)
                {
                    int rotation = Main.rand.Next(-60, 0);
                    var position = target.Center + new Vector2(Main.screenHeight + i * 50 + Main.rand.Next(-10, 10), i * 5).RotatedBy(MathHelper.ToRadians(-60)).RotatedBy(MathHelper.ToRadians(rotation));
                    var velocity = new Vector2(12, 0).RotatedBy(MathHelper.ToRadians(rotation - 180 - 60)) * (Main.rand.NextFloat(0.5f, 1f) + 1f);
                    Projectile.NewProjectile(Projectile.GetSource_NaturalSpawn(), position, velocity, ModContent.ProjectileType<HarpyFeather>(), hit.Damage / 3, hit.Knockback, Main.myPlayer);
                }

            }

            Projectile.NewProjectile(data, target.Center, new Vector2(0, 0), ModContent.ProjectileType<DamageZone>(), Projectile.damage / 2, 0, Projectile.owner);

            for (int i = 0; i < 20; i++)
            {
                Vector2 speed = Main.rand.NextVector2Unit();

                var position = target.Center;
                var dust = Dust.NewDustPerfect(position, DustID.Wet, new Vector2(0, 0), 75, default, 4f);
                dust.noGravity = true;
                dust.velocity = speed * 7;
            }
        }

        public override void AI()
        {
            base.CreateDust();
            base.AI();
        }
    }
}
