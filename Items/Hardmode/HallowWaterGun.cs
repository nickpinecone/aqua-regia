using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Items.Hardmode
{
    public class HallowWaterGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Shoots copies of itself");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 20;
            Item.knockBack = 5;

            Item.shootSpeed -= 6;
            Item.useTime += 30;
            // Item.useAnimation += 30;

            Item.shoot = ModContent.ProjectileType<Projectiles.Hardmode.HallowWaterProjectile>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = -1; i < 2; i += 2)
            {
                Vector2 modifiedVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(5));
                var offset = position + (modifiedVelocity * 14).RotatedBy(MathHelper.ToRadians(90 * i));
                Projectile.NewProjectile(source, offset, modifiedVelocity, type, damage, knockback, player.whoAmI);
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.HallowedBar, 18);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
