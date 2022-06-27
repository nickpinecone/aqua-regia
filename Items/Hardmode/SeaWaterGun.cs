using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Items.Hardmode
{
    public class SeaWaterGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Puts enemies in a bubble whirl");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 59;
            Item.knockBack = 5;
            Item.shoot = ModContent.ProjectileType<Projectiles.Hardmode.SeaWaterProjectile>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = -1; i < 2; i += 2)
            {
                int distanceBetween = 1;
                Vector2 modifiedVelocity = velocity.RotatedBy(MathHelper.ToRadians(distanceBetween * i * player.direction));
                var offset = new Vector2(position.X + velocity.X * 4, position.Y + velocity.Y * 4);
                Projectile.NewProjectile(source, offset, modifiedVelocity, type, damage, knockback, player.whoAmI);
            }

            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DukeFishronMask, 1);
            recipe.AddIngredient(ModContent.ItemType<PreHardmode.OceanWaterGun>(), 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
