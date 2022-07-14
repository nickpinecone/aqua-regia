using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Ammo
{
    public class StrongBottledWater : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Adds 100 damage\nDEV STUFF");
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.BottledWater);
            Item.ammo = ItemID.BottledWater;
        }
    }
}
