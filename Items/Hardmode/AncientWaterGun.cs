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
            for (int i = 0; i < Main.npc.Length; i++)
            {
                var distance = player.position - Main.npc[i].position;
                bool isVisible = Math.Abs(distance.X) < Main.ViewSize.X && Math.Abs(distance.Y) < Main.ViewSize.Y;
                if (Main.npc[i].life > 0 && isVisible)
                {
                    int rotation = Main.rand.Next(0, 360);
                    var randomPosition = Main.npc[i].Center + new Vector2(256, 0).RotatedBy(MathHelper.ToRadians(rotation));
                    var modifiedVelocity = new Vector2(10, 0).RotatedBy(MathHelper.ToRadians(rotation - 180));
                    modifiedVelocity.RotatedByRandom(MathHelper.ToRadians(10));

                    var proj = Projectile.NewProjectileDirect(source, randomPosition, modifiedVelocity, type, damage / 2, knockback, player.whoAmI);
                    proj.tileCollide = false;

                }
            }
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
    }
}
