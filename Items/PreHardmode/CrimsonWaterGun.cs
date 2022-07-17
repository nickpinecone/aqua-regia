using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;

namespace WaterGuns.Items.PreHardmode
{
    public class CrimsonWaterGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crimson Rainer");
            Tooltip.SetDefault("Rains from the sky");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 13;
            Item.knockBack = 2;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            base.defaultInaccuracy = 7;
            float offsetInaccuracy = CalculateAccuracy(0.4f);
            // Put it above the mouse
            // Could create complications if zoomed out too much
            // Projectiles will not reach all the way to the bottom
            position.Y -= Main.ViewSize.Y / 1.5f;
            position.X = Main.MouseWorld.X;

            for (int i = 0; i < 3; i++)
            {
                var modifiedVelocity = new Vector2(0, 14);
                position.X = position.RotatedByRandom(MathHelper.ToRadians(offsetInaccuracy)).X;

                base.SpawnProjectile(player, source, position, modifiedVelocity, type, damage, knockback);
            }

            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.CrimtaneBar, 16);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
