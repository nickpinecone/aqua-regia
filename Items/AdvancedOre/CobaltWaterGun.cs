using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Items.AdvancedOre
{
    public class CobaltWaterGun : BaseWaterGun
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 32;
            Item.knockBack = 4;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.CobaltBar, 18);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
