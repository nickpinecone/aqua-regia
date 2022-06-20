using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace WaterGuns.Projectiles.Hardmode
{
    public class TurretWaterProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;

            Projectile.timeLeft = 2400;
            Projectile.friendly = false;
            Projectile.hostile = false;
        }

        int delayMax = 15;
        int delay = 15;
        public override void AI()
        {
            base.AI();

            var distanceToMouse = new Vector2(Main.MouseWorld.X - Projectile.Center.X, Main.MouseWorld.Y - Projectile.Center.Y);
            distanceToMouse.Normalize();
            Projectile.spriteDirection = (Main.MouseWorld.Y - Projectile.position.Y > 0) ? 1 : -1;
            Projectile.rotation = Projectile.Center.AngleTo(Main.MouseWorld) - (Projectile.spriteDirection == 1 ? 0 : MathHelper.Pi);

            if (Main.mouseLeft && delay >= delayMax)
            {
                delay = 0;
                var velocity = distanceToMouse * 10;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<AdvancedOre.AdvancedWaterProjectile>(), 40, 3, Projectile.owner);
            }
            delay += 1;
        }
    }

    public class MiniWaterProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            AIType = ProjectileID.WaterGun;

            Projectile.timeLeft += 20;
        }

        public override void AI()
        {
            base.AI();
            base.CreateDust(default, 1f);
        }
    }
}
