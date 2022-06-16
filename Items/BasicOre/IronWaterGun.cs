using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Items.BasicOre
{
    public class IronWaterGun : BaseWaterGun
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 10;
            Item.knockBack = 2;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.IronBar, 16);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
