using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Items.PreHardmode
{
    public class DemonWaterGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Demonic Flow");
            Tooltip.SetDefault("Spawns an additional stream of water upon impact");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 16;
            Item.knockBack = 4;
            Item.shoot = ModContent.ProjectileType<Projectiles.PreHardmode.DemonWaterProjectile>();

            Item.useAnimation += 4;
            Item.useTime += 4;
            Item.shootSpeed += 4;
            base.defaultInaccuracy = 2f;


            base.offsetIndependent = new Vector2(0, -3);
            base.offsetAmount = new Vector2(4, 4);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-12, 0);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DemoniteBar, 18);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
