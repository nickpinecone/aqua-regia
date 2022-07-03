using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Accessories
{
    public class WaterGunUltimata : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("44% increase to water guns accuracy\n12% increase to water guns projectiles speed\nReleases water streams when hit");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<GlobalPlayer>().waterGunAccuracy = 0.44f;
            player.GetModPlayer<GlobalPlayer>().waterGunSpeed = 0.12f;
            player.GetModPlayer<GlobalPlayer>().waterGunShield = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessories.WaterGunEnhancer>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Accessories.WaterGunShield>(), 1);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();
        }
    }
}
