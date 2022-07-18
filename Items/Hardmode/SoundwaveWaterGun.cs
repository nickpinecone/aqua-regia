using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Items.Hardmode
{
    public class SoundwaveWaterGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fluid Soundwave");
            Tooltip.SetDefault("Shoots a powerful sound wave fused with water");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 63;
            Item.knockBack = 6;
            Item.shoot = ModContent.ProjectileType<Projectiles.Hardmode.SoundwaveWaterProjectile>();
            Item.useTime -= 4;
            Item.useAnimation -= 4;
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
