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

            Item.damage = 30;
            Item.knockBack = 5;

            Item.shootSpeed -= 6;
            Item.useTime += 50;

            Item.shoot = ModContent.ProjectileType<Projectiles.Hardmode.HallowWaterProjectile>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = -1; i < 2; i += 2)
            {
                Vector2 modifiedVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(5));
                var offset = position + (modifiedVelocity * 32).RotatedBy(MathHelper.ToRadians(90 * i));
                if (i == -1)
                {
                    Projectile.NewProjectile(source, offset, modifiedVelocity, ModContent.ProjectileType<Projectiles.Hardmode.MechanicalWaterProjectiles.RetinazerProjectile>(), damage, knockback, player.whoAmI);
                }
                else
                {
                    Projectile.NewProjectile(source, offset, modifiedVelocity, ModContent.ProjectileType<Projectiles.Hardmode.MechanicalWaterProjectiles.SpazmatismProjectile>(), damage, knockback, player.whoAmI);
                }
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.HallowedBar, 18);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
