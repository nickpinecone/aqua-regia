using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Terraria.Localization;
using Terraria.Audio;

namespace WaterGuns.Items.Hardmode
{
    public class AncientWaterGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            DisplayName.SetDefault("Ancient Geyser");
            Tooltip.SetDefault("Unleashes a geyser under your cursor\nFull Pump: Spawns a boulder sandstorm\nDrops from Golem");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 62;
            Item.knockBack = 5;
            Item.shoot = ModContent.ProjectileType<Projectiles.Hardmode.AncientWaterProjectile>();

            Item.useTime += 24;
            Item.useAnimation += 24;

            base.increasePumpLevel = true;
            base.maxPumpLevel = 18;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (pumpLevel >= maxPumpLevel)
            {
                Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType<Projectiles.Hardmode.BoulderSandstorm>(), damage, knockback, player.whoAmI);
            }
            else
            {
                SoundEngine.PlaySound(SoundID.Item14);

                var geyserPosition = new Vector2();
                int ai = 0;

                for (int k = 0; k < Main.ViewSize.Y; k += 16)
                {
                    if (k >= 16 * 20)
                    {
                        geyserPosition = Main.MouseWorld + new Vector2(0, 160);
                        ai = 1;
                        break;
                    }

                    var tilePosition = (Main.MouseWorld + new Vector2(0, k)).ToTileCoordinates();
                    if (Main.tile[tilePosition.X, tilePosition.Y].HasTile && Main.tile[tilePosition.X, tilePosition.Y].BlockType == BlockType.Solid)
                    {
                        geyserPosition = tilePosition.ToWorldCoordinates();
                        break;
                    }
                }

                var proj = Projectile.NewProjectileDirect(Projectile.GetSource_NaturalSpawn(), geyserPosition, new Vector2(0, 0), ModContent.ProjectileType<Projectiles.Hardmode.AncientGeyser>(), Item.damage * 2, 0, player.whoAmI, ai);
                proj.Bottom = geyserPosition;

                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 20; j++)
                    {
                        var positionLocal = geyserPosition + new Vector2((i - 2) * -14, 0);
                        var speed = new Vector2(0, -1).RotatedByRandom(MathHelper.ToRadians(3));

                        var dust = Dust.NewDustPerfect(positionLocal, DustID.Wet, new Vector2(0, 0), 75, default, 4f);
                        dust.noGravity = true;
                        dust.fadeIn = 1.5f;
                        dust.velocity = speed * Main.rand.Next(0, 40);
                    }
                }

                for (int i = 0; i < 20; i++)
                {
                    var speed = (new Vector2(0, -1)).RotatedByRandom(MathHelper.ToRadians(3));
                    var positionLocal = geyserPosition + new Vector2(Main.rand.Next(-32, 32), 0);

                    var dust = Dust.NewDustPerfect(positionLocal, DustID.Smoke, new Vector2(0, 0), 0, default, 3f);
                    dust.noGravity = true;
                    dust.fadeIn = 1.5f;
                    dust.velocity = speed * Main.rand.Next(0, 40);
                }


                for (int i = 0; i < 20; i++)
                {
                    var speed = (new Vector2(0, -1)).RotatedByRandom(MathHelper.ToRadians(3));
                    var positionLocal = geyserPosition + new Vector2(Main.rand.Next(-32, 32), 0);

                    var dust = Dust.NewDustPerfect(positionLocal, DustID.Flare, new Vector2(0, 0), 0, default, 3f);
                    dust.noGravity = true;
                    dust.fadeIn = 1.5f;
                    dust.velocity = speed * Main.rand.Next(0, 40);
                }
            }

            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(4, -2);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddCondition(NetworkText.FromLiteral("Mods.WaterGuns.Conditions.Never"), (_) => false);
            recipe.Register();
        }
    }
}
