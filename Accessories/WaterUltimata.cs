using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Accessories
{
    public class WaterUltimata : BaseAccessory
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("44% increase to water guns accuracy\n12% increase to water guns projectiles speed\nReleases water streams when hit\n12% increase to water guns damage\nAdds 4 defense");
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<GlobalPlayer>().waterGunAccuracy = 0.44f;
            player.GetModPlayer<GlobalPlayer>().waterGunSpeed = 0.12f;

            player.GetModPlayer<GlobalPlayer>().waterGunShield = true;
            player.GetModPlayer<GlobalPlayer>().waterGunDamage = 0.12f;
            player.statDefense += 4;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessories.WaterGunEnhancer>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Accessories.WaterProtector>(), 1);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();
        }
    }
}
