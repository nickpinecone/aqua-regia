using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Accessories
{
    public class WaterGunEnhancer : BaseAccessory
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("44% increase to water guns accuracy\n12% increase to water guns projectiles speed");
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<GlobalPlayer>().waterGunAccuracy = 0.44f;
            player.GetModPlayer<GlobalPlayer>().waterGunSpeed = 0.12f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessories.WaterGunScope>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Accessories.WaterGunAccelerator>(), 1);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();
        }
    }
}
