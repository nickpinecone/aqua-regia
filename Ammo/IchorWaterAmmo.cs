using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Ammo
{
    public class IchorWaterAmmo : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("inflicts ichor debuff");
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.BottledWater);
            Item.ammo = ItemID.BottledWater;
        }
    }
}
