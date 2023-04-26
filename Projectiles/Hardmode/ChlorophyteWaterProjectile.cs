using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.Hardmode
{
    public class ChlorophyteWaterProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            AIType = ProjectileID.WaterGun;

            Projectile.timeLeft += 20;
        }

        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);

            data.dustScale = 1;
        }

        public override void AI()
        {
            base.AI();
            base.CreateDust();
            base.AutoAim();
        }
    }
}
