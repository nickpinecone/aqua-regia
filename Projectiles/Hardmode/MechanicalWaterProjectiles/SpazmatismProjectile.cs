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
            hasKillEffect = false;
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

                WaterGuns.ProjectileData data = new WaterGuns.ProjectileData(Projectile.GetSource_FromThis());
                data.color = new Color(96, 248, 2);
                data.dustAmount = 2;
                data.dustScale = 2;
                data.fadeIn = 0;
                var proj = Projectile.NewProjectileDirect(data, offset, velocity, ModContent.ProjectileType<WaterProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                proj.tileCollide = false;
                proj.timeLeft -= 40;
            }
            delay += 1;

        }
    }
}
