using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Accessories
{
    public abstract class WaterStone : BaseAccessory
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("10% increase to range damage\nWater Shield does a bit more damage");
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Ranged) += 0.10f;
            player.GetModPlayer<GlobalPlayer>().waterStone = true;
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
