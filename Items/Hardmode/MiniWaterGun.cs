using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Items.Hardmode
{
    public class MiniWaterGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Rapid but inaccurate");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 45;
            Item.knockBack = 4;

            Item.useTime = 5;
            Item.useAnimation = 5;
            Item.shoot = ModContent.ProjectileType<Projectiles.Hardmode.MiniWaterProjectile>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 modifiedVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(8));
            Projectile.NewProjectile(source, position, modifiedVelocity, type, damage, knockback, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.ChainGun, 1);
            recipe.AddIngredient(ItemID.WaterBucket, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
