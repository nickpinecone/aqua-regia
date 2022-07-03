using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Accessories
{
    public class WaterGunShield : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Releases water streams when hit");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<GlobalPlayer>().waterGunShield = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddRecipeGroup("MoreWaterGuns:TitaniumBars", 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
