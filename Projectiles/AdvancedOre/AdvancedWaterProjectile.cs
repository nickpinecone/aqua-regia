using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.AdvancedOre
{
    public class AdvancedWaterProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.WaterGun);
            AIType = ProjectileID.WaterGun;
        }
    }
}
