using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Items.AdvancedOre
{
    public class TitaniumWaterGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("BasicSword"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            Tooltip.SetDefault("Shoots two projecitles in opposite directions");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.WaterGun);

            Item.damage = 40;
            Item.knockBack = 4;
            Item.shoot = ModContent.ProjectileType<Projectiles.AdvancedOre.AdvancedWaterProjectile>();
            Item.useTime -= 2;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // Make it a little inaccurate
            Vector2 modifiedVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(1));
            Projectile.NewProjectile(source, position, modifiedVelocity, type, damage, knockback, player.whoAmI);

            var secondPosition = new Vector2(position.X + modifiedVelocity.X * 2, position.Y + modifiedVelocity.Y * 2);
            var secondVelocity = modifiedVelocity.RotatedBy(MathHelper.ToRadians(180));
            Projectile.NewProjectile(source, secondPosition, secondVelocity, type, damage, knockback, player.whoAmI);

            return false;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 4);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.TitaniumBar, 18);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}

