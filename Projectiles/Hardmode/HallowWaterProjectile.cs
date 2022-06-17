using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.Hardmode
{
    public class WaterProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.WaterGun);
            AIType = ProjectileID.WaterGun;
        }
    }

    public class HallowWaterProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            // Projectile.CloneDefaults(ProjectileID.WaterGun);
            AIType = ProjectileID.WaterGun;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 80;
        }

        public override void AI()
        {
            base.AI();

            var distanceToMouse = new Vector2(Main.MouseWorld.X - Projectile.position.X, Main.MouseWorld.Y - Projectile.position.Y);
            distanceToMouse.Normalize();
            Projectile.rotation = Projectile.position.AngleTo(Main.MouseWorld);
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, distanceToMouse * 4, ModContent.ProjectileType<WaterProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner);

        }
    }
}
