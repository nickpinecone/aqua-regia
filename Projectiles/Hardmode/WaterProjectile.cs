using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.Hardmode
{
    public class WaterProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            AIType = ProjectileID.WaterGun;
        }

        int direction = 1;

        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
        }

        int delay = 0;
        int delayMax = 5;
        public override void AI()
        {
            base.AI();
            base.CreateDust();
        }
    }
}
