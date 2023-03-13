using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Items.PreHardmode
{
    public class GoldWaterGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Golden Water Splitter");
            Tooltip.SetDefault("Shoots two streams of water\nFull Pump: Water streams split mid-air");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 11;
            Item.knockBack = 3;

            base.defaultInaccuracy = 2;
            base.isOffset = true;
            base.offsetAmount = new Vector2(5f, 5f);
            base.offsetIndependent = new Vector2(0, -6);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var projType = 0;
            int distanceBetween = 2;

            if (pumpLevel >= maxPumpLevel)
            {
                projType = ModContent.ProjectileType<Projectiles.PreHardmode.SplitProjectile>();
                distanceBetween = 4;
                velocity = velocity / 1.3f;
            }
            else
            {
                projType = type;
            }

            var _pumpLevel = pumpLevel;
            for (int i = -1; i < 2; i += 2)
            {
                pumpLevel = _pumpLevel;
                Vector2 modifiedVelocity = velocity.RotatedBy(MathHelper.ToRadians(distanceBetween * i));
                base.SpawnProjectile(player, source, position, modifiedVelocity, projType, damage, knockback);
            }

            return false;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-12, 0);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddRecipeGroup("MoreWaterGuns:GoldBars", 20);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
