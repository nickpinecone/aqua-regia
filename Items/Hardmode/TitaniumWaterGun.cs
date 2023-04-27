using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Items.Hardmode
{
    public class TitaniumWaterGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            DisplayName.SetDefault("Titanium Riveria");
            Tooltip.SetDefault("Creates a damage zone on impact");
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            base.offsetIndependent = new Vector2(0, -3);
            base.offsetAmount = new Vector2(4, 4);

            Item.damage = 40;
            Item.knockBack = 4;
            Item.shoot = ModContent.ProjectileType<Projectiles.Hardmode.TitaniumWaterProjectile>();
            base.defaultInaccuracy = 1;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.TitaniumBar, 18);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();

            Recipe recipe2 = CreateRecipe();
            recipe2.AddIngredient(ItemID.AdamantiteBar, 18);
            recipe2.AddTile(TileID.MythrilAnvil);
            recipe2.Register();
        }
    }
}

