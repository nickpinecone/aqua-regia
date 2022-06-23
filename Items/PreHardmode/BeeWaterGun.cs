using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;

namespace WaterGuns.Items.PreHardmode
{
    public class BeeWaterGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Spawns bees");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 20;
            Item.knockBack = 4;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var modifiedVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(3.3f));
            Projectile.NewProjectile(source, position, modifiedVelocity, ProjectileID.Bee, 8, 1, player.whoAmI);
            Projectile.NewProjectile(source, position, modifiedVelocity, type, damage, knockback, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.BeeWax, 12);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
