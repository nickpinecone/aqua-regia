using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Items.Hardmode
{
    public class CursedWaterGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cursed Flow");
            Tooltip.SetDefault("Inflicts the cursed flames debuff");
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-4, -2);
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            base.offsetIndependent = new Vector2(0, -3);

            Item.damage = 43;
            Item.knockBack = 5;
            Item.shoot = ModContent.ProjectileType<Projectiles.Hardmode.CursedWaterProjectile>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.CursedFlame, 18);
            recipe.AddIngredient(ModContent.ItemType<PreHardmode.DemonWaterGun>(), 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
