using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace WaterGuns.Blocks
{
    public abstract class Machine : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Blocks.MachineTile>());
            Item.value = 150;
            Item.maxStack = 99;
            Item.width = 38;
            Item.height = 24;
        }
    }
}
