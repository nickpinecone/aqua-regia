using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace WaterGuns.Items.Hardmode
{
    public class SoundwaveWaterGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fluid Soundwave");
            Tooltip.SetDefault("Shoots a powerful sound wave fused with water");
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-24, 0);
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 63;
            Item.knockBack = 6;
            Item.shoot = ModContent.ProjectileType<Projectiles.Hardmode.SoundwaveWaterProjectile>();
            Item.useTime -= 4;
            Item.useAnimation -= 4;
            Item.scale = 0.7f;

            base.offsetAmount = new Vector2(7, 7);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.ShroomiteBar, 16);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
