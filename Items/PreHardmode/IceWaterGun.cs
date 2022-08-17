using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System.Collections.Generic;

namespace WaterGuns.Items.PreHardmode
{
    public class IceWaterGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ice Glacier");
            Tooltip.SetDefault("Consolidates an ice shard every third pump\nFull Pump: Ice shards turn into a frost wave");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 13;
            Item.knockBack = 3;
            base.offsetIndependent = new Vector2(0, -5);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-2, 0);
        }

        int numOfShots = 0;
        int maxNumOfShots = 3;
        List<Projectile> projs = new List<Projectile>();
        public override bool AltFunctionUse(Player player)
        {
            if (numOfShots < 2 && projs.Count < 3)
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
            return base.AltFunctionUse(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (pumpLevel >= 10)
            {
                var proj = Projectile.NewProjectileDirect(Projectile.GetSource_NaturalSpawn(), position, velocity, ProjectileID.FrostWave, Item.damage * 2, 4, player.whoAmI);
                proj.friendly = true;

                for (int i = 0; i < projs.Count; i++)
                {
                    projs[i].Kill();
                }
            }
            else
            {
                // Release the shards
                for (int i = 0; i < projs.Count; i++)
                {
                    velocity = velocity.RotatedByRandom(MathHelper.ToRadians(6));

                    projs[i].friendly = true;
                    projs[i].velocity = velocity;
                }

            }
            projs.Clear();
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.IceBlock, 10);
            recipe.AddIngredient(ItemID.SnowBlock, 30);
            recipe.AddRecipeGroup(RecipeGroupID.IronBar, 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
