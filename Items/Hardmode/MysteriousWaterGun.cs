using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;

using WaterGuns.Projectiles.PreHardmode;
using WaterGuns.Projectiles.Hardmode;

namespace WaterGuns.Items.Hardmode
{
    public class MysteriousWaterGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Unlimited Power");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 92;
            Item.knockBack = 6;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int[] projs = {
                ModContent.ProjectileType<SpaceWaterProjectile>(),
                ModContent.ProjectileType<LavaWaterProjectile>(),
                ModContent.ProjectileType<ChlorophyteWaterProjectile>(),
                ModContent.ProjectileType<CursedWaterProjectile>(),
                ModContent.ProjectileType<RainbowWaterProjectile>(),
                ModContent.ProjectileType<RocketWaterProjectile>(),
                ModContent.ProjectileType<SoundwaveWaterProjectile>(),
            };

            projs = projs.OrderBy(x => Main.rand.Next()).ToArray();
            for (int i = 0; i < projs.Length; i++)
            {
                var proj = base.SpawnProjectile(player, source, position, velocity.RotatedBy(MathHelper.ToRadians((i - projs.Length / 2) * 15)), projs[i], damage / 4, knockback);
                proj.penetrate = -1;
            }

            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.LunarBar, 14);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
