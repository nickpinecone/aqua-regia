using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Ammo
{
    public class EndlessBottledWater : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("DEV STUFF");
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.BottledWater);
            Item.ammo = ItemID.BottledWater;
            Item.consumable = false;
            Item.maxStack = 1;
        }
    }
}
