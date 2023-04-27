using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Terraria.Localization;

namespace WaterGuns.Items.Hardmode
{
    public class AncientWaterGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            DisplayName.SetDefault("Ancient Geyser");
            Tooltip.SetDefault("Unleashes a geyser on your enemies\nDrops from Golem");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 64;
            Item.knockBack = 5;
            Item.shoot = ModContent.ProjectileType<Projectiles.Hardmode.AncientWaterProjectile>();

            Item.useTime += 24;
            Item.useAnimation += 24;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(4, -2);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddCondition(NetworkText.FromLiteral("Mods.WaterGuns.Conditions.Never"), (_) => false);
            recipe.Register();
        }
    }
}
