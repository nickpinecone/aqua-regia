using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace WaterGuns.Items.PreHardmode
{
    public class SpaceWaterGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("BasicSword"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            Tooltip.SetDefault("Water bounces off the walls");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.WaterGun);

            Item.damage = 19;
            Item.knockBack = 4;
            Item.shoot = ModContent.ProjectileType<Projectiles.PreHardmode.SpaceWaterProjectile>();
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 4);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.CrimtaneBar, 16);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
