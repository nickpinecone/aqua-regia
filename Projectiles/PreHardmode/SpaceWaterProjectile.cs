using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.PreHardmode
{
    public class SpaceWaterProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            AIType = ProjectileID.WaterGun;
            Projectile.CloneDefaults(ProjectileID.WaterGun);

            Projectile.penetrate += 3;
        }

        int counter = 0;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (counter < 5)
            {
                counter += 1;
                Projectile.velocity = -oldVelocity.RotatedByRandom(MathHelper.ToRadians(55));
                return false;
            }
            return base.OnTileCollide(oldVelocity);
        }

        float gravity = 0.001f;
        public override void AI()
        {
            // Curve it like the in-game water gun projectile
            gravity += 0.002f;
            Projectile.velocity.Y += gravity;

            base.AI();
        }
    }
}
