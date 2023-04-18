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
            Projectile.timeLeft = 10;
            Projectile.width = 102;
            Projectile.height = 48;
        }

        Vector2 dist;
        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);

            dist = Main.player[Main.myPlayer].Center - Projectile.Center;
        }

        int delayMax = 10;
        int delay = 10;
        public override void AI()
        {
            base.AI();
            Projectile.Center = Main.player[Main.myPlayer].Center - dist;

            Projectile.timeLeft = 10;

            Projectile.spriteDirection = (Main.MouseWorld.X - Projectile.Center.X > 0) ? 1 : -1;
            Projectile.rotation = Projectile.Center.AngleTo(Main.MouseWorld) - (Projectile.spriteDirection == 1 ? 0 : MathHelper.Pi);

            var distanceToMouse = new Vector2(Main.MouseWorld.X - Projectile.Center.X, Main.MouseWorld.Y - Projectile.Center.Y);
            distanceToMouse.Normalize();

            if (Main.mouseLeft && delay >= delayMax)
            {
                delay = 0;
                var velocity = distanceToMouse * 10;
                var offset = new Vector2(Projectile.Center.X + velocity.X * 5, Projectile.Center.Y + velocity.Y * 5 + 2);

                base.data.dustAmount = 2;
                base.data.dustScale = 2;
                base.data.fadeIn = 0;
                var proj = Projectile.NewProjectileDirect(base.data, offset, velocity, ModContent.ProjectileType<WaterProjectile>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner);
                // proj.tileCollide = false;
                proj.timeLeft -= 25;
            }
            else if (!Main.mouseLeft)
            {
                Projectile.Kill();
            }

            delay += 1;
        }
    }
}
