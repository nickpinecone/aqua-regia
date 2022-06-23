using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace WaterGuns.Items.PreHardmode
{
    public class WoodenWaterGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault(
                "Additional acorn falls on an enemy, but only when trees are near.\n" +
                "'Prevents deforestation'"
            );
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 5;
            Item.knockBack = 0.8f;

            Item.useTime += 3;
            Item.shoot = ModContent.ProjectileType<Projectiles.PreHardmode.WoodenWaterProjectile>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Wood, 20);
            recipe.AddIngredient(ItemID.Acorn, 10);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
