using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Ammo.BottledWater
{
    public class EndlessBottledWater : BaseAmmo
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Magical And Endless");
            base.SetStaticDefaults();
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
