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
            base.defaultInaccuracy = 1;
            base.isOffset = false;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var proj1 = base.SpawnProjectile(player, source, position, velocity, type, damage, knockback);

            var secondPosition = new Vector2(position.X + proj1.velocity.X * 2, position.Y + proj1.velocity.Y * 2);
            var secondVelocity = proj1.velocity.RotatedBy(MathHelper.ToRadians(180));

            base.SpawnProjectile(player, source, secondPosition, secondVelocity, type, damage, knockback);

            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddRecipeGroup("MoreWaterGuns:TitaniumBars", 18);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}

