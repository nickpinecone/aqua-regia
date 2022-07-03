using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
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

            Item.damage = 47;
            Item.knockBack = 7;
            Item.shoot = ModContent.ProjectileType<Projectiles.Hardmode.WaterProjectile>();
            Item.useTime -= 4;
            Item.useAnimation -= 4;
        }

        int counter = 0;
        public override void HoldItem(Player player)
        {
            if (counter >= 1 && !Main.mouseLeft)
            {
                var distanceToMouse = new Vector2(Main.MouseWorld.X - player.Center.X, Main.MouseWorld.Y - player.Center.Y);
                distanceToMouse.Normalize();
                distanceToMouse *= 10 * CalculateSpeed();
                var offset = new Vector2(player.Center.X + distanceToMouse.X * 4, player.Center.Y + distanceToMouse.Y * 4);

                if (counter >= 10)
                {
                    Projectile.NewProjectileDirect(Projectile.GetSource_NaturalSpawn(), offset, distanceToMouse, ModContent.ProjectileType<Projectiles.Hardmode.ChargingWaterProjectiles.HugeWaterProjectile>(), (int)(Item.damage * 4f), Item.knockBack, player.whoAmI);
                }
                else if (counter >= 5)
                {
                    Projectile.NewProjectileDirect(Projectile.GetSource_NaturalSpawn(), offset, distanceToMouse, ModContent.ProjectileType<Projectiles.Hardmode.ChargingWaterProjectiles.MediumWaterProjectile>(), (int)(Item.damage * 2f), Item.knockBack, player.whoAmI);
                }
                else
                {
                    Projectile.NewProjectileDirect(Projectile.GetSource_NaturalSpawn(), offset, distanceToMouse, Item.shoot, Item.damage, Item.knockBack, player.whoAmI);
                }

                counter = 0;
            }
            base.HoldItem(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (counter < 10)
            {
                counter += 1;
                if (counter == 5)
                {
                    SoundEngine.PlaySound(SoundID.Item4);
                }
                if (counter >= 10)
                {
                    SoundEngine.PlaySound(SoundID.Item4);
                }
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Ruby, 1);
            recipe.AddIngredient(ItemID.Topaz, 1);
            recipe.AddIngredient(ItemID.Sapphire, 1);
            recipe.AddIngredient(ItemID.Amethyst, 1);
            recipe.AddIngredient(ItemID.Diamond, 1);
            recipe.AddIngredient(ItemID.Emerald, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
