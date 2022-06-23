using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Items.PreHardmode
{
    public class OceanWaterGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault(
                "Spawns additional bubbles" +
                "\n'It pops!'"
            );
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 10;
            Item.knockBack = 2;
            Item.shoot = ModContent.ProjectileType<Projectiles.PreHardmode.OceanWaterProjectile>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Seashell, 12);
            recipe.AddIngredient(ItemID.Starfish, 10);
            recipe.AddIngredient(ItemID.Coral, 8);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
