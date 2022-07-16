using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using System;

namespace WaterGuns.Projectiles.Hardmode
{
    public class WaterBallonProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            AIType = ProjectileID.Grenade;
            Projectile.timeLeft = 600;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, 30, 30, DustID.Wet, 0, 0, 75, default, 1.2f);
            }
            base.Kill(timeLeft);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = -oldVelocity.X;
            if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y;

            return false;
        }

        protected float gravity = 0.04f;
        public override void AI()
        {
            // Curve it like the in-game water gun projectile
            Projectile.velocity.Y += gravity;
            Projectile.rotation += MathHelper.ToRadians(10);
            Projectile.velocity *= 0.998f;

            base.AI();
        }
    }
}
