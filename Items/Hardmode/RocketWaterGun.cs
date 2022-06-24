using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Items.Hardmode
{
    public class RocketWaterGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Shoots rockets that explode into water projctiles");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 73;
            Item.knockBack = 5;
            Item.shoot = ModContent.ProjectileType<Projectiles.Hardmode.WaterProjectile>();
            Item.useTime -= 2;
        }

        int shot = 0;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            shot += 1;

            if (shot >= 4)
            {
                Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<Projectiles.Hardmode.RocketWaterProjectile>(), damage, knockback, player.whoAmI);
                shot = 0;
            }

            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.FragmentVortex, 18);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
