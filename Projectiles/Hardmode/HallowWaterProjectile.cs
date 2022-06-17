using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.Hardmode
{
    public class WaterProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            AIType = ProjectileID.WaterGun;
            Projectile.tileCollide = false;
            Projectile.timeLeft -= 20;
        }

        public override void AI()
        {
            base.AI();
            base.CreateDust(default, 1);
        }
    }

    public class HallowWaterProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            AIType = ProjectileID.WaterGun;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 140;
        }

        int delayMax = 30;
        int delay = 30;
        public override void AI()
        {
            base.AI();

            var distanceToMouse = new Vector2(Main.MouseWorld.X - Projectile.position.X, Main.MouseWorld.Y - Projectile.position.Y);
            distanceToMouse.Normalize();
            Projectile.rotation = Projectile.position.AngleTo(Main.MouseWorld);

            if (Main.mouseLeft && delay >= delayMax)
            {
                delay = 0;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, distanceToMouse * 10, ModContent.ProjectileType<WaterProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
            delay += 1;

        }
    }
}
