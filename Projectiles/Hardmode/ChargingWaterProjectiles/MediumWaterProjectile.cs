using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.Hardmode.ChargingWaterProjectiles
{
    public class MediumWaterProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            AIType = ProjectileID.WaterGun;
            Projectile.height += 8;
            Projectile.width += 8;
        }

        public override void Kill(int timeLeft)
        {
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<MediumExplosion>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            base.Kill(timeLeft);
        }

        public override void AI()
        {
            base.AI();
            base.CreateDust(default, 1.8f);
        }
    }
}
