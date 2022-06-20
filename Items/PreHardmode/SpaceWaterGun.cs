using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace WaterGuns.Items.PreHardmode
{
    public class SpaceWaterGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Water bounces off the walls and pierces enemies");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 17;
            Item.knockBack = 4;
            Item.shoot = ModContent.ProjectileType<Projectiles.PreHardmode.SpaceWaterProjectile>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // Custom projectile doesnt position right so offset it
            var offset = new Vector2(position.X + velocity.X * 4, position.Y + velocity.Y * 4);
            base.Shoot(player, source, offset, velocity, type, damage, knockback);
            return false;
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
