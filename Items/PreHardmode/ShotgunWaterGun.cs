using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace WaterGuns.Items.PreHardmode
{
    public class ShotgunWaterGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Shoots multiple streams of water");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 20;
            Item.knockBack = 6;
            Item.useAnimation += 12;
            Item.useTime += 12;

            base.defaultInaccuracy = 12;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = -1; i < 3; i++)
            {
                int distanceBetween = Main.rand.Next(6, 10);
                Vector2 modifiedVelocity = velocity.RotatedBy(MathHelper.ToRadians(distanceBetween * i * player.direction));
                var proj = base.SpawnProjectile(player, source, position, modifiedVelocity, type, damage, knockback);
                proj.timeLeft -= Main.rand.Next(15, 40);
            }

            return false;
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
