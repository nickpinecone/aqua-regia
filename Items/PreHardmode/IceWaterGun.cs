using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace WaterGuns.Items.PreHardmode
{
    public class IceWaterGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Consolidates an ice shard every fourth shot. Right click to release them");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 13;
            Item.knockBack = 3;
        }

        public override bool AltFunctionUse(Player player)
        {
            // Release the shards
            for (int i = 0; i < projs.Count; i++)
            {
                var velocity = Main.MouseWorld - player.position;
                velocity.Normalize();
                // Speed them up and make a bit inaccurate
                velocity *= 10;
                velocity = velocity.RotatedByRandom(MathHelper.ToRadians(6));

                projs[i].friendly = true;
                projs[i].velocity = velocity;
            }

            projs.Clear();
            return base.AltFunctionUse(player);
        }

        public override void HoldItem(Player player)
        {
            // Because while you shoot AltFunctionUse doesnt trigger for some reason
            // Found this workaround
            if (Main.mouseRight)
            {
                this.AltFunctionUse(player);
            }
            base.HoldItem(player);
        }

        int numOfShots = 0;
        int maxNumOfShots = 3;
        List<Projectile> projs = new List<Projectile>();
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (numOfShots < 3 && projs.Count < 3)
            {
                numOfShots += 1;
            }
            else
            {
                numOfShots = 0;

                if (projs.Count < 3)
                {
                    var proj = Projectile.NewProjectileDirect(Projectile.GetSource_NaturalSpawn(), player.position, new Vector2(0, 0), ModContent.ProjectileType<Projectiles.PreHardmode.IceWaterProjectile>(), Item.damage - 3, 4, player.whoAmI);
                    projs.Add(proj);
                }
            }

            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.IceBlock, 30);
            recipe.AddIngredient(ItemID.SnowBlock, 50);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
