using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Ammo
{
    public class BottledVenom : BaseAmmo
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            base.damage = 6;
            base.hasBuff = true;
            base.buffType = BuffID.Venom;
            base.buffTime = 240;
            base.color = new Color(173, 103, 230);

            Tooltip.SetDefault(damage + " ranged damage" + "\nInflicts Venom and explodes into litlle venom clouds");
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.BottledWater, 50);
            recipe.AddIngredient(ItemID.VialofVenom, 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.ReplaceResult(Item.type, 50);
            recipe.Register();
        }
    }
}
