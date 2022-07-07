using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace WaterGuns.Items.Hardmode
{
    public class CustomData : IEntitySource
    {
        public string Context { get { return ""; } }
        public int ProjSize { get { return 2; } }
    }

    public class ChlorophyteWaterGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Homes in on the foes");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 54;
            Item.knockBack = 5;
            Item.useTime -= 10;
            Item.useAnimation -= 10;

            Item.shoot = ModContent.ProjectileType<Projectiles.Hardmode.ChlorophyteWaterProjectile>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var customSource = new CustomData();
            Projectile.NewProjectile(customSource, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
            // return base.Shoot(player, customSource, position, velocity, type, damage, knockback);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.ChlorophyteBar, 18);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
