using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace WaterGuns.Projectiles.PreHardmode
{
    public class SimpleWaterProjectileWaterPunch : SimpleWaterProjectile
    {
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
            if (data.fullCharge)
            {
                Projectile waterFist = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Main.player[Main.myPlayer].Center, Vector2.Zero, ModContent.ProjectileType<WaterFistPunch>(), Projectile.damage * 2, 6, Projectile.owner);
                (waterFist.ModProjectile as WaterFistPunch).target = target;
            }
        }
    }

    public class WaterFistPunch : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 30;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 80;

            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.hostile = false;
            Projectile.scale = 1.3f;
        }

        int direction = -1;
        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);

            Projectile.ai[0] = 10f;
            Projectile.alpha = 255;

            if (Main.rand.Next(0, 2) == 0)
            {
                direction = -direction;
            }

            Projectile.spriteDirection = direction;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
            Projectile.Kill();

            for (int i = 0; i < 6; i++)
            {
                var speed = Main.rand.NextVector2Unit();
                var dust = Dust.NewDust(Projectile.Center, 16, 16, DustID.Cloud, 0, 0, 75, default, 3f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity = speed * 5;
            }
        }

        public NPC target = null;
        bool active = false;
        Vector2 animPosition = Vector2.Zero;
        int delay = -20;

        public override void AI()
        {
            base.AI();

            if (Projectile.ai[0] > 0f)
            {
                Projectile.ai[0] -= 1f;
                Projectile.alpha -= 255 / 10;
            }

            if (target != null && !active)
            {
                Projectile.Center = target.Center + new Vector2(0, -target.height) + animPosition;

                animPosition += new Vector2(0, (delay > 0) ? 8 : -4);
                if (direction == 1 && Projectile.rotation <= 0)
                {
                    Projectile.rotation += (delay > 0) ? 0.08f * direction : -0.04f * direction;
                }
                else if (direction == -1 && Projectile.rotation >= 0)
                {
                    Projectile.rotation += (delay > 0) ? 0.08f * direction : -0.04f * direction;
                }

                delay += 1;
            }
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

                Projectile.NewProjectile(data, Projectile.Center, velocity, ModContent.ProjectileType<Projectiles.PreHardmode.SimpleWaterProjectileWaterPunch>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
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

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (data.fullCharge)
            {
                Projectile waterFist = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Main.player[Main.myPlayer].Center, Vector2.Zero, ModContent.ProjectileType<WaterFistPunch>(), Projectile.damage * 2, 6, Projectile.owner);
                (waterFist.ModProjectile as WaterFistPunch).target = target;
            }
            base.OnHitNPC(target, hit, damageDone);
        }

        public override void OnKill(int timeLeft)
        {
            if (waterGun != null)
            {
                waterGun.Projectile.Kill();
            }
            base.OnKill(timeLeft);
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
