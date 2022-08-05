using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Items.PreHardmode
{
    public class LavaWaterGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lavashark");
            Tooltip.SetDefault("Sets your enemies ablaze");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            base.isOffset = true;
            base.defaultInaccuracy = 3f;
            base.offsetAmount = new Vector2(6, 6);

            Item.damage = 31;
            Item.knockBack = 5;
            Item.shoot = ModContent.ProjectileType<Projectiles.PreHardmode.LavaWaterProjectile>();
            Item.useTime -= 8;
            Item.useAnimation -= 8;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-34, -2);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.HellstoneBar, 16);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
