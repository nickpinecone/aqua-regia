using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace WaterGuns.Items.Hardmode
{
    public class AncientWaterGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Geyser");
            Tooltip.SetDefault("Knows exactly where your enemies are");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 64;
            Item.knockBack = 5;
            Item.shoot = ModContent.ProjectileType<Projectiles.Hardmode.WaterProjectile>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // Spawn projectiles from the ground solid block
            base.defaultInaccuracy = 4;
            for (int i = 0; i < Main.npc.Length; i++)
            {
                var distance = player.position - Main.npc[i].position;
                bool isVisible = Math.Abs(distance.X) < Main.ViewSize.X && Math.Abs(distance.Y) < Main.ViewSize.Y;

                if (Main.npc[i].life > 0 && isVisible)
                {
                    var randomPosition = Vector2.Zero;
                    var rotation = Main.rand.Next(-16, 16);

                    for (int k = 0; k < Main.ViewSize.Y; k += 16)
                    {
                        var tilePosition = (Main.npc[i].Center + new Vector2(0, k).RotatedBy(MathHelper.ToRadians(rotation))).ToTileCoordinates();
                        if (Main.tile[tilePosition.X, tilePosition.Y].HasTile && Main.tile[tilePosition.X, tilePosition.Y].BlockType == BlockType.Solid)
                        {
                            randomPosition = tilePosition.ToWorldCoordinates() + new Vector2(0, 32).RotatedBy(MathHelper.ToRadians(rotation));
                            break;
                        }
                    }
                    var modifiedVelocity = new Vector2(0, -10).RotatedBy(MathHelper.ToRadians(rotation));

                    var proj = base.SpawnProjectile(player, source, randomPosition, modifiedVelocity, type, damage, knockback);
                    proj.tileCollide = false;
                }
            }
            base.defaultInaccuracy = 1;
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
    }
}
