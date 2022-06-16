using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Items.BasicOre
{
    public class SilverWaterGun : BaseWaterGun
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

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
