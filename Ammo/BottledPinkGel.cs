using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Ammo
{
    public class BottledPinkGel : BaseAmmo
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("2 ranged damage\nMakes projectiles bounce");
            base.SetStaticDefaults();
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.BottledWater, 50);
            recipe.AddIngredient(ItemID.PinkGel, 10);
            recipe.AddTile(TileID.WorkBenches);
            recipe.ReplaceResult(Item.type, 50);
            recipe.Register();
        }
    }
}
