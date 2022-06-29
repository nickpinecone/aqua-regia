using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns
{
    public class GlobalPlayer : ModPlayer
    {
        // 1 = stat not changed, 0 = stat maxed
        public float waterGunAcuraccy = 1;
        public float waterGunSpeed = 1;
        public float waterGunRange = 1;

        public float CalculateAccuracy(float defaultInaccuracy)
        {
            return defaultInaccuracy * waterGunAcuraccy;
        }
    }
}
