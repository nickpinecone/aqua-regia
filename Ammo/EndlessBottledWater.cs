using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Ammo
{
    public class EndlessBottledWater : BaseAmmo
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Tooltip.SetDefault(damage + " ranged damage" + "\nMagical And Endless");

            base.damage = 1;
        }
        public override void SetDefaults()
        {
            Item.ammo = ItemID.BottledWater;
            Item.height = 20;
            Item.width = 20;
            Item.consumable = false;
            Item.maxStack = 1;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.BottledWater, 999);
            recipe.AddTile(TileID.CrystalBall);
            recipe.Register();
        }
    }
}
