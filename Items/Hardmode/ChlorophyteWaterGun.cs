using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace WaterGuns.Items.Hardmode
{
    public class ChlorophyteWaterGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chlorophyte Water Sprayer");
            Tooltip.SetDefault("Chases after your foes");
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-12, -4);
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            base.offsetAmount = new Vector2(5, 5);
            base.offsetIndependent = new Vector2(0, -5);

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
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
