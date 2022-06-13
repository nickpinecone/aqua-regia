using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Items
{
    public class SilverWaterGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("BasicSword"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            Tooltip.SetDefault("'Take that rusty water!'");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.WaterGun);

            Item.damage = 12;
            Item.knockBack = 2;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SilverBar, 16);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
