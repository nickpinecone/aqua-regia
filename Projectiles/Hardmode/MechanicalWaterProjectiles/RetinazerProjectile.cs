using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.Hardmode.MechanicalWaterProjectiles
{
    public class RetinazerProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            AIType = ProjectileID.WaterGun;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 160;
            Projectile.scale = 0.8f;

            gravity = 0.04f;
        }

        int delayMax = 40;
        int delay = 40;
        public override void AI()
        {
            base.AI();

            Projectile.velocity.Y += gravity;

            var distanceToMouse = new Vector2(Main.MouseWorld.X - Projectile.position.X, Main.MouseWorld.Y - Projectile.position.Y);
            distanceToMouse.Normalize();
            Projectile.rotation = Projectile.position.AngleTo(Main.MouseWorld);

            if (Main.mouseLeft && delay >= delayMax)
            {
                delay = 0;
                var velocity = distanceToMouse * 10;
                var offset = new Vector2(Projectile.position.X + velocity.X * 4, Projectile.position.Y + velocity.Y * 4);

                var proj = Projectile.NewProjectileDirect(base.data, offset, velocity, ModContent.ProjectileType<WaterProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                proj.tileCollide = false;
                proj.timeLeft += 30;
            }
            delay += 1;

        }
    }
}
