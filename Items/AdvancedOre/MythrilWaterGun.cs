using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Items.BasicOre
{
    public class MythrilWaterGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("BasicSword"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.WaterGun);

            Item.damage = 25;
            Item.knockBack = 4;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 4);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.CobaltBar, 18);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
