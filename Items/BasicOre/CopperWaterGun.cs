using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Items.BasicOre
{
    public class CopperWaterGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("'Take that rusty water!'");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 8;
            Item.knockBack = 1;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.CopperBar, 18);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
