
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Global
{
    public class GlobalProjectileList : GlobalProjectile
    {
        public override void SetDefaults(Projectile projectile)
        {
            if (projectile.type == ProjectileID.WaterGun)
            {
                projectile.damage = 1;
                projectile.penetrate = 1;

                projectile.width = 16;
                projectile.height = 16;

                projectile.friendly = true;
                projectile.hostile = false;
            }
        }
    }
}
