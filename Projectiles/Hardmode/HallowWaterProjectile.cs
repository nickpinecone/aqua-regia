using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.Hardmode
{
    public class HallowWaterProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.timeLeft -= 10;
        }

        public override void AI()
        {
            base.AI();
            base.CreateDust();
        }
    }
}
