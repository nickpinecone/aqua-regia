using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace WaterGuns.Blocks
{
    public class Salt : ModItem
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.maxStack = 999;
            Item.consumable = true;
            // Item.createTile = ModContent.TileType<SaltRock>();
            Item.width = 26;
            Item.height = 24;
            Item.value = 500;
        }
    }
}
