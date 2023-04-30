using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace WaterGuns.Items.PreHardmode
{
    public class SpaceWaterGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            DisplayName.SetDefault("Aqua Zapper");
            Tooltip.SetDefault("Water bounces off the walls and pierces enemies\nFull Pump: Spawns a water laser from space upon your enemies");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            base.isOffset = true;
            base.offsetAmount = new Vector2(5, 5);
            base.offsetIndependent = new Vector2(0, -5);

            Item.damage = 18;
            Item.knockBack = 4;
            Item.shoot = ModContent.ProjectileType<Projectiles.PreHardmode.SpaceWaterProjectile>();
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-16, -2);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.MeteoriteBar, 22);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
