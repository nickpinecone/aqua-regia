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

        int projSize = 1;
        public override void OnSpawn(IEntitySource source)
        {
            int size = ((Items.Hardmode.CustomData)source).ProjSize;
            projSize = size;
            base.OnSpawn(source);
        }

        public override void AI()
        {
            base.AI();
            base.CreateDust(default, projSize);
            base.AutoAim();
        }
    }
}
