using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Ammo
{
    public class BottledHolyWater : BaseAmmo
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            base.spawnsStar = true;
            base.damage = 2;

            Tooltip.SetDefault(damage + " ranged damage" + "\nMakes a star fall on the enemy");
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.BottledWater, 50);
            recipe.AddIngredient(ItemID.FallenStar, 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.ReplaceResult(Item.type, 50);
            recipe.Register();
        }
    }
}
