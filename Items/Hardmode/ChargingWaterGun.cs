using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Items.Hardmode
{
    public class ChargingWaterGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("The longer is charges, the harder it hits");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 43;
            Item.knockBack = 5;
            Item.shoot = ModContent.ProjectileType<Projectiles.Hardmode.WaterProjectile>();
        }

        int counter = 0;
        public override void HoldItem(Player player)
        {
            if (counter >= 1 && !Main.mouseLeft)
            {
                var distanceToMouse = new Vector2(Main.MouseWorld.X - player.position.X, Main.MouseWorld.Y - player.position.Y);
                distanceToMouse.Normalize();
                distanceToMouse *= 10;
                var proj = Projectile.NewProjectileDirect(Projectile.GetSource_NaturalSpawn(), player.Center, distanceToMouse, Item.shoot, Item.damage, Item.knockBack, player.whoAmI);

                if (counter >= 5)
                {
                    proj.height *= 2;
                    proj.width *= 2;
                }

                counter = 0;
            }
            base.HoldItem(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            counter += 1;
            return false;
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
