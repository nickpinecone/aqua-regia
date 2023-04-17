using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Items.Hardmode
{
    public class SpectralWaterGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Water Wisperer");
            Tooltip.SetDefault("Hitting enemies spawns lost souls");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            base.offsetAmount = new Vector2(5, 5);
            base.offsetIndependent = new Vector2(0, -4);
            base.increasePumpLevel = false;

            Item.damage = 55;
            Item.knockBack = 5;
            Item.shoot = ModContent.ProjectileType<Projectiles.Hardmode.SpectralWaterProjectile>();

            Item.useTime += 4;
            Item.useAnimation += 4;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-26, -6);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SpectreBar, 18);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
