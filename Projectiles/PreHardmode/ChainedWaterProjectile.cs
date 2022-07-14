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
            Projectile.width = 8;
            Projectile.height = 8;

            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.scale = 0.9f;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.rotation = Main.rand.NextFloat(0, MathHelper.TwoPi);
            base.OnSpawn(source);
        }

        int delay = 0;
        public override void AI()
        {
            if (delay > 10)
            {
                var velocity = new Vector2(10, 0).RotatedBy(Projectile.rotation);

                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, velocity, ModContent.ProjectileType<Projectiles.PreHardmode.SimpleWaterProjectile>(), 20, 3, Projectile.owner);
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
            waterGun = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, ModContent.ProjectileType<WaterGunProjectile>(), 0, 0, Projectile.owner);
            base.OnSpawn(source);
        }

        public override void Kill(int timeLeft)
        {
            waterGun.Kill();
            base.Kill(timeLeft);
        }

        public override void AI()
        {
            waterGun.Center = new Vector2(Projectile.position.X - MathF.Abs(Projectile.velocity.X), Projectile.position.Y - MathF.Abs(Projectile.velocity.Y));
            base.AI();
        }
    }
}
