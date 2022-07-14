using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace WaterGuns.Projectiles.PreHardmode
{
    public class SimpleWaterProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            AIType = ProjectileID.WaterGun;
        }
    }
}
