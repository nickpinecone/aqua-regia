using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.PreHardmode
{
    public class WaterFountain : BaseProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            base.defaultDust = false;

            Projectile.timeLeft = 120;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.friendly = false;
            Projectile.extraUpdates = 0;
        }


        int delay = 10;
        public override void AI()
        {
            delay += 1;
            if (delay > 10)
            {
                delay = 0;
                Projectile.rotation += 0.6f;
                var velocity = new Vector2(10, 0).RotatedBy(Projectile.rotation);
                Projectile.NewProjectile(base.data, Projectile.Center, velocity, ModContent.ProjectileType<Projectiles.PreHardmode.SimpleWaterProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
        }

    }

    public class DemonWaterProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            // Projectile.CloneDefaults(ProjectileID.WaterGun);
            base.SetDefaults();
            AIType = ProjectileID.WaterGun;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (data.fullCharge)
            {
                var modifiedVelocity = Vector2.Zero;
                var randomPosition = target.Center;

                Projectile.NewProjectile(base.data, randomPosition, modifiedVelocity, ModContent.ProjectileType<Projectiles.PreHardmode.WaterFountain>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
            else
            {

                int rotation = Main.rand.Next(-90, 0);
                var randomPosition = target.Center + new Vector2(256, 0).RotatedBy(MathHelper.ToRadians(-45)).RotatedBy(MathHelper.ToRadians(rotation));
                var modifiedVelocity = new Vector2(10, 0).RotatedBy(MathHelper.ToRadians(rotation - 180 - 45));

                // Spawn default water projectile
                Projectile.NewProjectile(base.data, randomPosition, modifiedVelocity, ModContent.ProjectileType<Projectiles.PreHardmode.SimpleWaterProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
            base.OnHitNPC(target, damage, knockback, crit);
        }
    }
}
