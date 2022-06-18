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
            Tooltip.SetDefault("Consolidates an ice shard every fourth shot. Press right click to release them");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 11;
            Item.knockBack = 3;
        }

        public override bool AltFunctionUse(Player player)
        {
            for (int i = 0; i < projs.Count; i++)
            {
                var velocity = Main.MouseWorld - player.position;
                velocity.Normalize();
                velocity *= 10;
                velocity = velocity.RotatedByRandom(MathHelper.ToRadians(6));

                projs[i].friendly = true;
                projs[i].velocity = velocity;
            }

            projs.Clear();
            return base.AltFunctionUse(player);
        }

        List<Projectile> projs = new List<Projectile>();

        int numOfShots = 0;
        int maxNumOfShots = 3;
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
                    var proj = Projectile.NewProjectileDirect(Projectile.GetSource_NaturalSpawn(), player.position, new Vector2(0, 0), ModContent.ProjectileType<Projectiles.PreHardmode.IceWaterProjectile>(), 6, 4, player.whoAmI);
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
