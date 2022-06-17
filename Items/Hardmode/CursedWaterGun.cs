using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Items.Hardmode
{
    public class CursedWaterGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Inflicts cursed flames debuff");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 43;
            Item.knockBack = 5;
            Item.shoot = ModContent.ProjectileType<Projectiles.Hardmode.CursedWaterProjectile>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.CursedFlame, 18);
            recipe.AddIngredient(ModContent.ItemType<PreHardmode.DemonWaterGun>(), 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
