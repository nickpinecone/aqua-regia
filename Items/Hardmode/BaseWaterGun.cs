using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace WaterGuns.Items.Hardmode
{
    public abstract class BaseWaterGun : CommonWaterGun
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            base.defaultInaccuracy = 1f;

            Item.useTime -= 2;
            Item.useAnimation -= 2;

            increasePumpLevel = false;
        }
    }
}
