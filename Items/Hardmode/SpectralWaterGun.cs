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

        public override bool AltFunctionUse(Player player)
        {
            var velocity = -Main.MouseWorld.DirectionTo(player.position);
            velocity.Normalize();
            velocity *= 14;

            for (int i = 0; i < soulsList.Count; i++)
            {
                var newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(20f));

                soulsList[i].friendly = true;
                soulsList[i].velocity = newVelocity;
            }
            soulsList.Clear();


            if (pumpLevel > 0)
            {
                if (pumpLevel >= 10)
                    pumpLevel = 0;
                else
                {
                    pumpLevel -= 2;
                    if (pumpLevel < 0)
                        pumpLevel = 0;
                }
            }

            return false;
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
