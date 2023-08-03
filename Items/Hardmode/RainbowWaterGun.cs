using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace WaterGuns.Items.Hardmode
{
    public class RainbowWaterGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-4, -2);
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            base.offsetIndependent = new Vector2(0, -2);

            Item.damage = 41;
            Item.knockBack = 5;
            Item.shoot = ModContent.ProjectileType<Projectiles.Hardmode.RainbowWaterProjectile>();
            base.increasePumpLevel = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddCondition(LocalizedText.Empty, () => false);
            recipe.Register();
        }
    }
}
