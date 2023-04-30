
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Ammo
{
    public class BottledUnholyWater : BaseAmmo
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            base.damage = 2;
            base.penetrates = true;
            base.color = new Color(0, 194, 129);

            Tooltip.SetDefault(damage + " ranged damage" + "\nAllows to penetrate up to 3 enemies");
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
