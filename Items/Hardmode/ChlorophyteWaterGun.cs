using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System;

namespace WaterGuns.Items.Hardmode
{
    public class ChlorophyteWaterGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            DisplayName.SetDefault("Chlorophyte Water Sprayer");
            Tooltip.SetDefault("Chases after your foes\nFull Pump: Releases Man Eaters that cling onto enemies");
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-12, -4);
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            base.offsetAmount = new Vector2(5, 5);
            base.offsetIndependent = new Vector2(0, -5);

            Item.damage = 49;
            Item.knockBack = 5;
            Item.useTime -= 8;
            Item.useAnimation -= 8;

            Item.shoot = ModContent.ProjectileType<Projectiles.Hardmode.ChlorophyteWaterProjectile>();

            base.increasePumpLevel = true;
            base.maxPumpLevel = 12;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (pumpLevel >= maxPumpLevel)
            {
                float detectRange = MathF.Pow(1200f, 2);

                Vector2 vel = Vector2.Zero;
                int count = 0;
                for (int i = 0; i < Main.npc.Length; i++)
                {
                    if (count >= 3)
                    {
                        break;
                    }

                    NPC target = Main.npc[i];

                    if (!target.CanBeChasedBy() || target.Center.DistanceSQ(player.Center) > detectRange)
                    {
                        continue;
                    }


                    vel = player.Center.DirectionTo(target.Center);
                    vel.Normalize();
                    vel *= 14;

                    count += 1;

                    Projectile.NewProjectile(source, position, vel, ModContent.ProjectileType<Projectiles.Hardmode.PlantClinger>(), (int)(damage / 1.6f), 0, player.whoAmI);
                }

                // Release three man eaters always
                if (count != 0)
                {
                    for (int i = 0; i < 3 - count; i++)
                    {
                        Vector2 newVel = vel.RotatedByRandom(MathHelper.ToRadians(10f));
                        Projectile.NewProjectile(source, position, newVel, ModContent.ProjectileType<Projectiles.Hardmode.PlantClinger>(), (int)(damage / 1.6f), 0, player.whoAmI);
                    }
                }
            }


            return base.Shoot(player, source, position, velocity, type, damage, knockback);
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
