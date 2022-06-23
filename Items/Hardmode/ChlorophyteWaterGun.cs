using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Items.Hardmode
{
    public class ChlorophyteWaterGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Homes in on the foes");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 54;
            Item.knockBack = 5;
            Item.useTime -= 10;
            Item.useAnimation -= 10;

            Item.shoot = ModContent.ProjectileType<Projectiles.Hardmode.ChlorophyteWaterProjectile>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.ChlorophyteBar, 18);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}