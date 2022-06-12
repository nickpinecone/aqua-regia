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

            Item.damage = 800;
            Item.knockBack = 2;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            position.Y -= 448;
            for (int i = -1; i < 2; i++)
            {
                velocity = new Vector2(Main.MouseWorld.X - position.X, Main.MouseWorld.Y - position.Y + ((Main.MouseWorld.X - position.X) / 16 * (Main.MouseWorld.X - position.X > 0 ? -1 : 1)));
                velocity = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.ToRadians(5 * i));
                velocity.Normalize();
                velocity *= 16;
                Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
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
