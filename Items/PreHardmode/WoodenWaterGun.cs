using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Items.PreHardmode
{
    public class WoodenWaterGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 5;
            Item.knockBack = 0.8f;

            Item.useTime += 3;
            Item.useAnimation += 3;

            Item.shoot = ModContent.ProjectileType<Projectiles.PreHardmode.WoodenWaterProjectile>();

            base.isOffset = true;
            base.offsetAmount = new Vector2(3.4f, 3.4f);
            base.offsetIndependent = new Vector2(0, -0.8f);
            base.maxPumpLevel = 7;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Wood, 20);
            recipe.AddIngredient(ItemID.Acorn, 5);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
