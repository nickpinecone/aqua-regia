using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.Hardmode.MechanicalWaterProjectiles
{
    public class SpazmatismProjectile : RetinazerProjectile
    {
        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);

            delay = 10;
            delayMax = 10;
            spaz = true;
            timeOffset = -30;
        }
    }
}
