using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Items.AdvancedOre
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
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // Make it a little inaccurate
            Vector2 modifiedVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(1));
            Projectile.NewProjectile(source, position, modifiedVelocity, type, damage, knockback, player.whoAmI);

            var secondPosition = new Vector2(position.X + modifiedVelocity.X * 2, position.Y + modifiedVelocity.Y * 2);
            var secondVelocity = modifiedVelocity.RotatedBy(MathHelper.ToRadians(180));
            Projectile.NewProjectile(source, secondPosition, secondVelocity, type, damage, knockback, player.whoAmI);

            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.TitaniumBar, 18);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}

