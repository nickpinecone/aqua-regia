using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using System;

namespace WaterGuns.Projectiles.PreHardmode
{
    public class SpaceWaterProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Projectile.penetrate = 2;
        }

        int counter = 0;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (counter < 4)
            {
                counter += 1;

                // Bounce off the wall without creating a new projectile
                var velocity = -oldVelocity.RotatedByRandom(MathHelper.ToRadians(45));
                Projectile.velocity = velocity;

                // Reset gravity and timeLeft so it doesnt destroy
                Projectile.timeLeft = 62;
                base.gravity = 0.001f;
                return false;
            }
            return base.OnTileCollide(oldVelocity);
        }

        public override void AI()
        {
            base.AI();

            // // Creating some dust to see the projectile
            base.CreateDust(default, 1.2f);
        }
    }
}
