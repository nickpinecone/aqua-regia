using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Ammo
{
    public class StrongBottledWater : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.BottledWater);
            Item.ammo = ItemID.BottledWater;
        }
    }
}
