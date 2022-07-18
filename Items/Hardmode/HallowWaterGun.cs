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
            DisplayName.SetDefault("Hallowed Fisher");
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
            base.defaultInaccuracy = 5;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-28, -4);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = -1; i < 2; i += 2)
            {
                var offset = position + (velocity * 32).RotatedBy(MathHelper.ToRadians(90 * i));
                if (i == -1)
                {
                    base.SpawnProjectile(player, source, offset, velocity, ModContent.ProjectileType<Projectiles.Hardmode.MechanicalWaterProjectiles.RetinazerProjectile>(), damage, knockback);
                }
                else
                {
                    base.SpawnProjectile(player, source, offset, velocity, ModContent.ProjectileType<Projectiles.Hardmode.MechanicalWaterProjectiles.SpazmatismProjectile>(), damage, knockback);
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
