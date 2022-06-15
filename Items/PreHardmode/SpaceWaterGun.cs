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

            Item.damage = 17;
            Item.knockBack = 4;
            Item.shoot = ModContent.ProjectileType<Projectiles.PreHardmode.SpaceWaterProjectile>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 modifiedVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(3));
            Projectile.NewProjectile(source, new Vector2(position.X + velocity.X * 4, position.Y + velocity.Y * 4), modifiedVelocity, type, damage, knockback, player.whoAmI);
            return false;
            // return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }


        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 4);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.MeteoriteBar, 22);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
