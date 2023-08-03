using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace WaterGuns.Items.Hardmode
{
    public class WaterBalloonGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 31;
            Item.knockBack = 3;
            Item.shoot = ModContent.ProjectileType<Projectiles.Hardmode.WaterBalloonProjectile>();
            Item.shootSpeed -= 6;
            Item.value = Item.buyPrice(0, 20, 50, 0);

            base.isOffset = true;
            base.offsetAmount = new Vector2(8, 8);
            base.offsetIndependent = new Vector2(0, -6);

            base.increasePumpLevel = true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddCondition(LocalizedText.Empty, () => false);
            recipe.Register();
        }
    }
}

