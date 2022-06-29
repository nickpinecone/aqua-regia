using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns
{
    public class GlobalPlayer : ModPlayer
    {
        // 0 = stat not changed, 1 = stat maxed
        public float waterGunAccuracy = 0;
        public float waterGunSpeed = 0;
        public float waterGunRange = 0;

        public float CalculateAccuracy(float defaultInaccuracy)
        {
            return defaultInaccuracy - defaultInaccuracy * waterGunAccuracy;
        }

        public float CalculateSpeed(float defaultSpeed)
        {
            return defaultSpeed + defaultSpeed * waterGunSpeed;
        }

        public float CalculateRange(float defaultRange)
        {
            return defaultRange + defaultRange * waterGunRange;
        }

        public override void ResetEffects()
        {
            waterGunAccuracy = 0;
            waterGunSpeed = 0;
            waterGunRange = 0;
        }
    }
}
