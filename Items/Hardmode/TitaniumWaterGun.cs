using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Items.Hardmode
{
    public class TitaniumWaterGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Shoots two projecitles in opposite directions");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 40;
            Item.knockBack = 4;
            Item.shoot = ModContent.ProjectileType<Projectiles.Hardmode.TitaniumWaterProjectile>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // All of them use custom projectiles that shoot straight 
            // Make them a little inaccurate like in-game water gun
            Vector2 modifiedVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(1));
            // Custom projectile doesnt position right so offset it
            Projectile.NewProjectile(source, position, modifiedVelocity, type, damage, knockback, player.whoAmI);

            // Make it inaccurate, get it closer to player and rotate
            modifiedVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(1));
            var secondPosition = new Vector2(position.X + modifiedVelocity.X * 2, position.Y + modifiedVelocity.Y * 2);
            var secondVelocity = modifiedVelocity.RotatedBy(MathHelper.ToRadians(180));

            Projectile.NewProjectile(source, secondPosition, secondVelocity, type, damage, knockback, player.whoAmI);

            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.TitaniumBar, 18);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}

