using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Items.PreHardmode
{
    public class SpaceWaterGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Water bounces off the walls and pierces enemies");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            base.isOffset = true;

            Item.damage = 17;
            Item.knockBack = 4;
            Item.shoot = ModContent.ProjectileType<Projectiles.PreHardmode.SpaceWaterProjectile>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.MeteoriteBar, 22);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
