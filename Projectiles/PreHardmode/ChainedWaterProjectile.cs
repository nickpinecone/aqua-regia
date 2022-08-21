using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace WaterGuns.Projectiles.PreHardmode
{
    public class WaterGunMine : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;

            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 180;
            Projectile.penetrate = -1;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.rotation = Main.rand.NextFloat(0, MathHelper.TwoPi);
            base.OnSpawn(source);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            applyGravity = false;
            Projectile.velocity = Vector2.Zero;
            return false;
        }

        bool applyGravity = true;
        int delay = 0;
        float gravity = 0.1f;

        public override void AI()
        {
            if (applyGravity)
            {
                gravity += 0.2f;
                Projectile.position.Y += gravity;
                if (Projectile.velocity.X > 0)
                {
                    Projectile.rotation += 0.16f;
                }
                else
                {
                    Projectile.rotation -= 0.16f;
                }
            }

            if (delay > 30)
            {
                for (int i = 0; i < 4; i++)
                {
                    var velocity = new Vector2(10, 0).RotatedBy(Projectile.rotation + i * MathHelper.PiOver2);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<Projectiles.PreHardmode.SimpleWaterProjectile>(), 20, 3, Projectile.owner);
                }
                delay = 0;
            }
            delay += 1;
            base.AI();
        }
    }

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

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.rotation = Main.rand.NextFloat(0, MathHelper.TwoPi);
            base.OnSpawn(source);
        }

        int delay = 0;
        public override void AI()
        {
            if (delay > 12)
            {
                for (int i = 0; i < 4; i++)
                {
                    var velocity = new Vector2(10, 0).RotatedBy(Projectile.rotation + i * MathHelper.PiOver2);

                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<Projectiles.PreHardmode.SimpleWaterProjectile>(), 20, 3, Projectile.owner);
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

        Projectile waterGun = null;
        public override void OnSpawn(IEntitySource source)
        {
            waterGun = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<WaterGunProjectile>(), 0, 0, Projectile.owner);
            base.OnSpawn(source);
        }

        public override void Kill(int timeLeft)
        {
            waterGun.Kill();
            base.Kill(timeLeft);
        }

        public override void AI()
        {
            waterGun.position = new Vector2(Projectile.position.X - MathF.Abs(Projectile.velocity.X), Projectile.position.Y - MathF.Abs(Projectile.velocity.Y));
            base.AI();
        }
    }
}
