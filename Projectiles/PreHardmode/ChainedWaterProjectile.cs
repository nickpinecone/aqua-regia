using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace WaterGuns.Projectiles.PreHardmode
{
    public class WaterGunProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;

            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.scale = 1.0f;
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

                Projectile.NewProjectile(data, Projectile.Center, velocity, ModContent.ProjectileType<Projectiles.PreHardmode.SimpleWaterProjectile>(), 20, 3, Projectile.owner);
            }
        }

        public int delay = 0;
        public int maxDelay = 20;
        public override void AI()
        {
            if (delay > maxDelay && maxDelay != 0)
            {
                Shoot();
                delay = 0;
            }
            delay += 1;

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
            if (spinning)
            {
                Projectile.Kill();
                return;
            }

            if (source is WaterGuns.ProjectileData newData)
            {
                data = newData;
            }
            else
            {
                data = new WaterGuns.ProjectileData(source);
            }

            initVel = Projectile.velocity;

            var proj = Projectile.NewProjectileDirect(data, Projectile.Center, Vector2.Zero, ModContent.ProjectileType<WaterGunProjectile>(), 0, 0, Projectile.owner);
            waterGun = proj.ModProjectile as WaterGunProjectile;

            if (data.fullCharge)
            {
                degreeTurn *= Main.player[Main.myPlayer].direction;
                Projectile.position = Main.player[Main.myPlayer].position + turn;
            }

            base.OnSpawn(source);
        }

        public override void Kill(int timeLeft)
        {
            if (waterGun != null)
            {
                waterGun.Projectile.Kill();
            }
            base.Kill(timeLeft);
        }

        int delay = 0;
        Vector2 turn = new Vector2(0, -60);
        int degreeTurn = 20;
        bool haveShot = false;
        static bool spinning = false;
        public override void AI()
        {
            base.AI();

            if (initVel != Projectile.velocity && !haveShot && !spinning)
            {
                waterGun.Shoot();
                haveShot = true;
            }

            if (data.fullCharge)
            {
                spinning = true;
                waterGun.maxDelay = 12;
                data.dustScale = 2.4f;
                data.dustAmount = 1;

                if (delay >= 80)
                {
                    spinning = false;
                    Kill(0);
                    Projectile.Kill();
                }
                waterGun.Projectile.position = Projectile.position;
                Projectile.velocity = Vector2.Zero;
                turn = turn.RotatedBy(MathHelper.ToRadians(degreeTurn));
                Projectile.position = Main.player[Main.myPlayer].position + turn;
                delay += 1;
            }
            else
            {
                waterGun.maxDelay = 0;
                waterGun.Projectile.position = new Vector2(Projectile.position.X - MathF.Abs(Projectile.velocity.X), Projectile.position.Y - MathF.Abs(Projectile.velocity.Y));
            }

        }
    }
}
