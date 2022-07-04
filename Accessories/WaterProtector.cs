using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Accessories
{
    public class WaterProtector : BaseAccessory
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("12% increase to water guns damage\nAdds 4 defense\nReleases water streams when hit");
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<GlobalPlayer>().waterGunShield = true;
            player.GetModPlayer<GlobalPlayer>().waterGunDamage = 0.12f;
            player.statDefense += 4;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessories.WaterStone>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Accessories.WaterShield>(), 1);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();
        }
    }
}
