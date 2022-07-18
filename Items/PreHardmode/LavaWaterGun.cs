using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Items.PreHardmode
{
    public class LavaWaterGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lava Evaporator");
            Tooltip.SetDefault("Sets your enemies ablaze");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            base.isOffset = true;
            base.defaultInaccuracy = 3f;

            Item.damage = 31;
            Item.knockBack = 5;
            Item.shoot = ModContent.ProjectileType<Projectiles.PreHardmode.LavaWaterProjectile>();
            Item.useTime -= 8;
            Item.useAnimation -= 8;
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
