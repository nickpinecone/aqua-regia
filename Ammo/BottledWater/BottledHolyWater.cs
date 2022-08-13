using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Ammo.BottledWater
{
    public class BottledHolyWater : BaseAmmo
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("-WIP- BottledHolyWater");
            Tooltip.SetDefault("Holy Molly");
            base.SetStaticDefaults();
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.BottledWater, 50);
            recipe.AddIngredient(ItemID.DirtBlock, 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.ReplaceResult(Item.type, 50);
            recipe.Register();
        }
    }
}
