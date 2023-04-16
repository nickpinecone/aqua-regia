using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System.Collections.Generic;
using Terraria.Localization;

namespace WaterGuns.Items.PreHardmode
{
    public class IceWaterGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ice Glacier");
            Tooltip.SetDefault("Consolidates ice shards that form a shield around the player\nFull Pump: Releases the ice shards\nDrops from Deerclops");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 26;
            Item.knockBack = 4;
            Item.useTime -= 6;
            Item.useAnimation -= 6;
            base.offsetIndependent = new Vector2(0, -5);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-2, 0);
        }

        List<Projectile> projs = new List<Projectile>();
        public override void HoldItem(Player player)
        {
            if ((projs.Count + 1) * 2 < pumpLevel)
            {
                var proj = Projectile.NewProjectileDirect(Projectile.GetSource_NaturalSpawn(), player.position, new Vector2(0, 0), ModContent.ProjectileType<Projectiles.PreHardmode.IceWaterProjectile>(), (int)(Item.damage * 1.2f), 0, player.whoAmI);
                projs.Add(proj);
            }
            base.HoldItem(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (pumpLevel >= maxPumpLevel)
            {
                // var proj = Projectile.NewProjectileDirect(Projectile.GetSource_NaturalSpawn(), position, velocity, ModContent.ProjectileType<Projectiles.PreHardmode.FrostWave>(), Item.damage * 2, 4, player.whoAmI);
                // var proj2 = Projectile.NewProjectileDirect(Projectile.GetSource_NaturalSpawn(), position, velocity, ModContent.ProjectileType<Projectiles.PreHardmode.FrostWave>(), Item.damage * 2, 3, player.whoAmI);
                // Release the shards
                for (int i = 0; i < projs.Count; i++)
                {
                    velocity = velocity.RotatedByRandom(MathHelper.ToRadians(4));

                    projs[i].friendly = true;
                    projs[i].velocity = velocity;
                    projs[i].penetrate = 1;
                    projs[i].knockBack = 3;
                }
                projs.Clear();
            }

            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddCondition(NetworkText.FromLiteral("Mods.WaterGuns.Conditions.Never"), (_) => false);
            recipe.Register();
        }
    }
}
