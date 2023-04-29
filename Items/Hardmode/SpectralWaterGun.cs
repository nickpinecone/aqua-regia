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
            base.SetStaticDefaults();

            DisplayName.SetDefault("Water Wisperer");
            Tooltip.SetDefault("Killed enemies release a soul that comes to the player, boosting weapon damage");
        }

        public const int DELAY_MAX = 70;

        public int delay = 0;
        public int delayMax = DELAY_MAX;

        public override void UpdateInventory(Player player)
        {
            base.UpdateInventory(player);

            if (Item.damage > damage)
            {
                delay += 1;

                if (delay > delayMax)
                {
                    delayMax = (int)(delayMax / 1.15f);
                    delay = 0;
                    Item.damage -= 1;
                }
            }
        }

        public int damage = 60;
        public override void SetDefaults()
        {
            base.SetDefaults();
            base.offsetAmount = new Vector2(5, 5);
            base.offsetIndependent = new Vector2(0, -4);
            base.increasePumpLevel = false;

            Item.damage = damage;
            Item.knockBack = 5;
            Item.shoot = ModContent.ProjectileType<Projectiles.Hardmode.SpectralWaterProjectile>();

            Item.useTime += 6;
            Item.useAnimation += 6;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-26, -6);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var dist = new Vector2(Main.rand.Next(0, 16), 0).RotatedBy(MathHelper.ToRadians(Main.rand.Next(0, 360))) + new Vector2(0, -4);

            var projVelocity = -Main.MouseWorld.DirectionTo(player.Center);
            projVelocity.Normalize();
            projVelocity *= 8;
            var proj = Projectile.NewProjectileDirect(Projectile.GetSource_NaturalSpawn(), player.Center + dist, projVelocity, ModContent.ProjectileType<Projectiles.Hardmode.SoulProjectile>(), damage, 2, player.whoAmI);

            return base.Shoot(player, source, position, velocity, type, damage, knockback);
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
