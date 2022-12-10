using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Accessories
{
    public abstract class WaterUltimata : BaseAccessory
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Super Soaker");
            Tooltip.SetDefault("44% increase to water guns accuracy\n12% increase to water guns projectiles speed\n12% increase to ranged damage\nReleases water streams when hit\nAdds 5 defense\nWater Shield does a bit more damage");
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<GlobalPlayer>().waterGunAccuracy = 0.44f;
            player.GetModPlayer<GlobalPlayer>().waterGunSpeed = 0.12f;

            player.GetDamage(DamageClass.Ranged) += 0.12f;
            player.GetModPlayer<GlobalPlayer>().waterStone = true;
            player.GetModPlayer<GlobalPlayer>().waterShield = true;
            player.statDefense += 5;
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
