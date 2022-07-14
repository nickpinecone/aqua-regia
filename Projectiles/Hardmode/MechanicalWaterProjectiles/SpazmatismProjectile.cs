using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.Hardmode.MechanicalWaterProjectiles
{
    public class SpazmatismProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            AIType = ProjectileID.WaterGun;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 160;
        }

        int delayMax = 10;
        int delay = 10;
        public override void AI()
        {
            base.AI();

            var distanceToMouse = new Vector2(Main.MouseWorld.X - Projectile.position.X, Main.MouseWorld.Y - Projectile.position.Y);
            distanceToMouse.Normalize();
            Projectile.rotation = Projectile.position.AngleTo(Main.MouseWorld);

            if (Main.mouseLeft && delay >= delayMax)
            {
                delay = 0;
                var velocity = distanceToMouse * 10;
                var offset = new Vector2(Projectile.position.X + velocity.X * 4, Projectile.position.Y + velocity.Y * 4);

                base.data.dustAmount = 2;
                base.data.dustScale = 2;
                base.data.fadeIn = 0;
                var proj = Projectile.NewProjectileDirect(base.data, offset, velocity, ModContent.ProjectileType<WaterProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                proj.tileCollide = false;
                proj.timeLeft -= 40;
            }
            delay += 1;

        }
    }
}
