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
        public int hitCount = 0;
        public List<Projectile> soulsList = new List<Projectile> { };

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Water Wisperer");
            Tooltip.SetDefault("Hitting enemies increases the weapon power. Right click to release it");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            base.offsetAmount = new Vector2(5, 5);
            base.offsetIndependent = new Vector2(0, -4);
            base.decreasePumpLevel = false;

            Item.damage = 55;
            Item.knockBack = 5;
            Item.shoot = ModContent.ProjectileType<Projectiles.Hardmode.SpectralWaterProjectile>();
            Item.useTime -= 3;
            Item.useAnimation -= 3;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (pumpLevel >= maxPumpLevel)
            {
                var projVelocity = -Main.MouseWorld.DirectionTo(player.position);
                velocity.Normalize();
                velocity *= 14;

                for (int i = 0; i < soulsList.Count; i++)
                {
                    var newVelocity = projVelocity.RotatedByRandom(MathHelper.ToRadians(20f));

                    soulsList[i].friendly = true;
                    soulsList[i].velocity = newVelocity;
                }
                soulsList.Clear();

                pumpLevel = 0;
            }

            return base.Shoot(player, source, position, velocity, type, damage, knockback);
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
