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
            data.dustAmount -= 1;

            Projectile.rotation = Main.rand.NextFloat(0, MathHelper.TwoPi);
            base.OnSpawn(source);
        }

        int delay = 6;
        public override void AI()
        {
            if (delay > 12)
            {
                for (int i = 0; i < 4; i++)
                {
                    var velocity = new Vector2(10, 0).RotatedBy(Projectile.rotation + i * MathHelper.PiOver2);

                    Projectile.NewProjectile(data, Projectile.Center, velocity, ModContent.ProjectileType<Projectiles.PreHardmode.SimpleWaterProjectile>(), 20, 3, Projectile.owner);
                }
                delay = 0;
            }
            Projectile.rotation = Projectile.rotation + 0.1f;
            delay += 1;
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

        public WaterGuns.ProjectileData data = null;
        Projectile waterGun = null;
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

            waterGun = Projectile.NewProjectileDirect(data, Projectile.Center, Vector2.Zero, ModContent.ProjectileType<WaterGunProjectile>(), 0, 0, Projectile.owner);

            if (data.fullCharge)
            {
                degreeTurn *= Main.player[Main.myPlayer].direction;
                Projectile.position = Main.player[Main.myPlayer].position + turn;
            }

            base.OnSpawn(source);
        }

        public override void Kill(int timeLeft)
        {
            waterGun.Kill();
            base.Kill(timeLeft);
        }

        int delay = 0;
        Vector2 turn = new Vector2(0, -60);
        int degreeTurn = 20;
        public override void AI()
        {
            base.AI();

            if (data.fullCharge)
            {
                data.dustScale = 2.4f;
                data.dustAmount = 1;

                if (delay >= 80)
                {
                    Kill(0);
                    Projectile.Kill();
                }
                waterGun.position = Projectile.position;
                Projectile.velocity = Vector2.Zero;
                turn = turn.RotatedBy(MathHelper.ToRadians(degreeTurn));
                Projectile.position = Main.player[Main.myPlayer].position + turn;
                delay += 1;
            }
            else
            {
                waterGun.position = new Vector2(Projectile.position.X - MathF.Abs(Projectile.velocity.X), Projectile.position.Y - MathF.Abs(Projectile.velocity.Y));
            }

        }
    }
}
