using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Ammo.BottledWater
{
    public class BottledPoison : BaseAmmo
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("4 ranged damage\nInflicts poison");
            base.SetStaticDefaults();
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.BottledWater, 50);
            recipe.AddIngredient(ItemID.RottenChunk, 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.ReplaceResult(Item.type, 50);
            recipe.Register();

            Recipe recipe2 = CreateRecipe();
            recipe2.AddIngredient(ItemID.BottledWater, 50);
            recipe2.AddIngredient(ItemID.Vertebrae, 1);
            recipe2.AddTile(TileID.WorkBenches);
            recipe2.ReplaceResult(Item.type, 50);
            recipe2.Register();
        }
    }
}
