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

            Projectile.friendly = false;
            Projectile.hostile = false;
        }

        int delayMax = 15;
        int delay = 15;
        bool turned = false;
        public override void AI()
        {
            base.AI();

            var distanceToMouse = new Vector2(Main.MouseWorld.X - Projectile.position.X, Main.MouseWorld.Y - Projectile.position.Y);
            distanceToMouse.Normalize();
            Projectile.rotation = Projectile.Center.AngleTo(Main.MouseWorld);

            if (Main.mouseLeft && delay >= delayMax)
            {
                delay = 0;
                var velocity = distanceToMouse * 10;
                var offset = new Vector2(Projectile.position.X + MathF.Abs(velocity.X * 2), Projectile.position.Y + MathF.Abs(velocity.Y * 2)) + (Projectile.velocity * 1.5f).RotatedBy(90);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), offset, velocity, ModContent.ProjectileType<AdvancedOre.AdvancedWaterProjectile>(), 40, 3, Projectile.owner);
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
