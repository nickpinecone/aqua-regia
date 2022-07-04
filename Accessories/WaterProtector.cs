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
            Tooltip.SetDefault("12% increase to ranged damage\nAdds 5 defense\nReleases water streams when hit");
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<GlobalPlayer>().waterShield = true;
            player.GetDamage(DamageClass.Ranged) += 0.12f;
            player.statDefense += 5;
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
