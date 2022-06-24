using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Items.Hardmode
{
    public class RainbowWaterGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Spawns water streams downwards");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            base.isOffset = false;

            Item.damage = 46;
            Item.knockBack = 5;
            Item.shoot = ModContent.ProjectileType<Projectiles.Hardmode.RainbowWaterProjectile>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.PixieDust, 18);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
