using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Ammo.BottledWater
{
    public abstract class BaseAmmo : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.BottledWater);
            Item.potion = false;
            Item.healLife = 0;
            Item.ammo = ItemID.BottledWater;
        }
        public override bool? UseItem(Player player)
        {
            player.AddBuff(BuffID.Confused, 240);
            return true;
        }
    }
}
