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
            Item.useTime -= 8;
            Item.useAnimation -= 8;
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

            Vector2 modifiedVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(4));
            var offset = new Vector2(position.X + velocity.X * 4, position.Y + velocity.Y * 4);
            var proj = Projectile.NewProjectileDirect(source, offset, modifiedVelocity, type, damage, knockback, player.whoAmI);
            proj.timeLeft += 20;
            proj.penetrate = 2;

            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.FragmentVortex, 18);
            recipe.AddTile(412); // Ancient manipulator
            recipe.Register();
        }
    }
}
