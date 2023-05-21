using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Items.Hardmode
{
    public class HallowWaterGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            DisplayName.SetDefault("Hallowed Fisher");
            Tooltip.SetDefault("Spawns clones of itself\nFull Pump: Turn full skeletron prime mode");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 30;
            Item.knockBack = 5;

            Item.shoot = ModContent.ProjectileType<Projectiles.Hardmode.HallowWaterProjectile>();
            base.defaultInaccuracy = 2;
            base.offsetAmount = new Vector2(6, 6);

            base.increasePumpLevel = true;
            base.maxPumpLevel = 24;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-28, -4);
        }

        public override void HoldItem(Player player)
        {
            base.HoldItem(player);

            if (!Main.mouseLeft && spaz != null && ret != null)
            {
                spaz.Projectile.Kill();
                spaz = null;

                ret.Projectile.Kill();
                ret = null;
            }
        }


        Projectiles.Hardmode.MechanicalWaterProjectiles.SpazmatismProjectile spaz = null;
        Projectiles.Hardmode.MechanicalWaterProjectiles.RetinazerProjectile ret = null;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (pumpLevel >= maxPumpLevel)
            {
                var offset = player.Top + new Vector2(player.width * 4 * 1, -64);
                var proj = base.SpawnProjectile(player, source, offset, Vector2.Zero, ModContent.ProjectileType<Projectiles.Hardmode.MechanicalWaterProjectiles.LauncherProjectile>(), damage, knockback, false);

                offset = player.Top + new Vector2(player.width * 4 * -1, -64);
                // proj = base.SpawnProjectile(player, source, offset, Vector2.Zero, ModContent.ProjectileType<Projectiles.Hardmode.MechanicalWaterProjectiles.SpearProjectile>(), damage, knockback, false);
                Projectile.NewProjectileDirect(source, offset, Vector2.Zero, ModContent.ProjectileType<Projectiles.Hardmode.MechanicalWaterProjectiles.SpearProjectile>(), damage, knockback, player.whoAmI);
            }

            if (spaz == null || ret == null)
            {
                for (int i = -1; i < 2; i += 2)
                {
                    var offset = player.Top + new Vector2(player.width * 4 * i, -64);

                    if (i == player.direction)
                    {
                        var proj = base.SpawnProjectile(player, source, offset, Vector2.Zero, ModContent.ProjectileType<Projectiles.Hardmode.MechanicalWaterProjectiles.SpazmatismProjectile>(), damage, knockback);
                        spaz = proj.ModProjectile as Projectiles.Hardmode.MechanicalWaterProjectiles.SpazmatismProjectile;
                    }
                    else
                    {
                        var proj = base.SpawnProjectile(player, source, offset, Vector2.Zero, ModContent.ProjectileType<Projectiles.Hardmode.MechanicalWaterProjectiles.RetinazerProjectile>(), damage, knockback);
                        ret = proj.ModProjectile as Projectiles.Hardmode.MechanicalWaterProjectiles.RetinazerProjectile;
                    }
                }
            }

            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.HallowedBar, 18);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
