using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Accessories
{
    public class WaterStone : BaseAccessory
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("12% increase to water guns damage");
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<GlobalPlayer>().waterGunDamage = 0.12f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.HellstoneBar, 12);
            recipe.AddIngredient(ItemID.BottledWater, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
