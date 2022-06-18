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
            Tooltip.SetDefault("An additional acorn falls on an enemy, but only above ground");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 5;
            Item.knockBack = 0.5f;

            Item.useTime += 4;
            Item.shoot = ModContent.ProjectileType<Projectiles.PreHardmode.WoodenWaterProjectile>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Wood, 20);
            recipe.AddIngredient(ItemID.Acorn, 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
