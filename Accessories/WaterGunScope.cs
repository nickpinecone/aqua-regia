using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Accessories
{
    public class WaterGunScope : BaseAccessory
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("40% increase to water guns accuracy");
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<GlobalPlayer>().waterGunAccuracy = 0.4f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Glass, 20);
            recipe.AddRecipeGroup("MoreWaterGuns:GoldBars", 14);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
