using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Accessories
{
    public class WaterGunAccelerator : BaseAccessory
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("10% increase to water guns projectiles speed");
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<GlobalPlayer>().waterGunSpeed = 0.1f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.IronBar, 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
