using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Items.PreHardmode
{
    public class ChainedWaterGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Literally a gun on a chain\nFull Pump: Throws a spike that shoots water in four directions");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ChainGuillotines);
            Item.useTime *= 2;
            Item.useAnimation *= 2;

            Item.DamageType = DamageClass.Ranged;
            Item.autoReuse = true;
            Item.damage = 25;
            Item.knockBack = 4;
            Item.shoot = ModContent.ProjectileType<Projectiles.PreHardmode.ChainedWaterProjectile>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (pumpLevel >= maxPumpLevel)
            {
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<Projectiles.PreHardmode.WaterGunMine>(), damage, knockback, player.whoAmI);
            }
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Chain, 3);
            recipe.AddIngredient(ItemID.IronBar, 20);
            recipe.AddIngredient(ItemID.Hook, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
