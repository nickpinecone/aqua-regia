using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace WaterGuns.Projectiles.PreHardmode
{
    public class WaterFistPunch : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 30;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 48;

            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.hostile = false;
            Projectile.scale = 1.0f;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);

            Main.player[Main.myPlayer].velocity = Main.player[Main.myPlayer].velocity * -0.6f;
            Projectile.Kill();

            for (int i = 0; i < 6; i++)
            {
                var speed = Main.rand.NextVector2Unit();
                var dust = Dust.NewDust(Projectile.Center, 16, 16, DustID.Cloud, 0, 0, 75, default, 3f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity = speed * 5;
            }

        }

        public override void AI()
        {
            base.AI();

            var player = Main.player[Main.myPlayer];
            Projectile.Center = Main.player[Main.myPlayer].Center + player.velocity.SafeNormalize(Vector2.Zero) * 32;
            Projectile.rotation = Main.player[Main.myPlayer].velocity.ToRotation();
        }
    }

    public class WaterGunProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 34;

            Projectile.friendly = false;
            Projectile.hostile = false;
        }

        public WaterGuns.ProjectileData data = null;
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

            Projectile.rotation = Main.rand.NextFloat(0, MathHelper.TwoPi);
            base.OnSpawn(source);
        }

        public void Shoot()
        {
            for (int i = 0; i < 4; i++)
            {
                var velocity = new Vector2(10, 0).RotatedBy(Projectile.rotation + i * MathHelper.PiOver2);

                Projectile.NewProjectile(data, Projectile.Center, velocity, ModContent.ProjectileType<Projectiles.PreHardmode.SimpleWaterProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.rotation + 0.1f;

            base.AI();
        }
    }

    public class ChainedWaterProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.ChainGuillotine);
            AIType = ProjectileID.ChainGuillotine;
        }

        Vector2 initVel = Vector2.Zero;
        public WaterGuns.ProjectileData data = null;
        WaterGunProjectile waterGun = null;
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

            initVel = Projectile.velocity;


            var proj = Projectile.NewProjectileDirect(data, Projectile.Center, Vector2.Zero, ModContent.ProjectileType<WaterGunProjectile>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner);
            waterGun = proj.ModProjectile as WaterGunProjectile;

            base.OnSpawn(source);
        }

        Projectile waterFist = null;
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (data.fullCharge && waterFist == null)
            {
                waterFist = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Main.player[Main.myPlayer].Center, Vector2.Zero, ModContent.ProjectileType<WaterFistPunch>(), Projectile.damage, 6, Projectile.owner);

                var dir = Main.player[Main.myPlayer].Center.DirectionTo(target.Center);
                dir.Normalize();
                Main.player[Main.myPlayer].velocity = dir * 20;
            }

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            if (waterGun != null)
            {
                waterGun.Projectile.Kill();
            }
            base.Kill(timeLeft);
        }

        bool haveShot = false;
        public override void AI()
        {
            base.AI();

            if (initVel != Projectile.velocity && !haveShot)
            {
                waterGun.Shoot();
                haveShot = true;
            }

            waterGun.Projectile.Center = new Vector2(Projectile.Center.X, Projectile.Center.Y - 4);
        }
    }
}
