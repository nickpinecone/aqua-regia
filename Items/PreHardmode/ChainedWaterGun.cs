using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Items.PreHardmode
{
    public class ChainedWaterGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Literally a gun on a chain");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ChainGuillotines);
            Item.useTime *= 2;
            Item.useAnimation *= 2;

            Item.DamageType = DamageClass.Ranged;
            Item.autoReuse = true;
            Item.damage = 25;
            Item.knockBack = 4;
            Item.shoot = ModContent.ProjectileType<Projectiles.PreHardmode.ChainedWaterProjectile>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Chain, 3);
            recipe.AddIngredient(ItemID.IronBar, 20);
            recipe.AddIngredient(ItemID.Hook, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
