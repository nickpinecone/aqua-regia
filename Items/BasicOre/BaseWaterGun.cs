using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Items.BasicOre
{
    public abstract class BaseWaterGun : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.WaterGun);
        }

        public override Vector2? HoldoutOffset()
        {
            // So holding guns look like in-game water gun
            return new Vector2(0, 4);
        }
    }
}
