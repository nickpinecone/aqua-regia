using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace WaterGuns.Items.Hardmode
{
    public class CorruptBubbleGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            base.defaultInaccuracy = 16f;

            Item.scale *= 1.2f;
            Item.damage = 36;
            Item.knockBack = 2;
            Item.useTime -= 8;
            Item.useAnimation -= 8;

            Item.shoot = ModContent.ProjectileType<Projectiles.Hardmode.CorruptBubbleProjectile>();

            base.offsetIndependent = new Vector2(0, -4);
            base.increasePumpLevel = true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-2, -4);
        }

        public override void HoldItem(Player player)
        {
            base.HoldItem(player);
        }

        public int pumpShots = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (pumpLevel >= maxPumpLevel)
            {
                pumpShots = 3;
            }

            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddCondition(LocalizedText.Empty, () => false);
            recipe.Register();
        }
    }
}
