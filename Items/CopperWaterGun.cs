using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Items
{
    public class CopperWaterGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("BasicSword"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            Tooltip.SetDefault("'Take that rusty water!'");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.WaterGun);

            Item.damage = 8;
            Item.knockBack = 2;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 2; i++)
            {
                Vector2 offset = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.ToRadians(10 * i));
                Projectile.NewProjectile(source, position, offset, type, damage, knockback, player.whoAmI);
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.CopperBar, 18);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
