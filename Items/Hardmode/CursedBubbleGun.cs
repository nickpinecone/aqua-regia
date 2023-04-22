using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace WaterGuns.Items.Hardmode
{
    public class CursedBubbleGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cursed Bubble Popper");
            Tooltip.SetDefault("Shoots cursed bubbles that inflict Cursed Inferno\nDrops from Clinger");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            base.defaultInaccuracy = 16f;

            Item.scale *= 1.5f;
            Item.damage = 38;
            Item.knockBack = 2;
            Item.useTime -= 8;
            Item.useAnimation -= 8;

            Item.shoot = ModContent.ProjectileType<Projectiles.Hardmode.CursedBubbleProjectile>();

            // base.increasePumpLevel = true;
            // base.maxPumpLevel = 16;
        }

        public override void HoldItem(Player player)
        {
            base.HoldItem(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddCondition(NetworkText.FromLiteral("Mods.WaterGuns.Conditions.Never"), (_) => false);
            recipe.Register();
        }
    }
}
