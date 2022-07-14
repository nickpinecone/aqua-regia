using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Ammo
{
    public class BathWaterAmmo : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Deals mental damage");
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.BottledWater);
            Item.ammo = ItemID.BottledWater;
        }
    }
}
