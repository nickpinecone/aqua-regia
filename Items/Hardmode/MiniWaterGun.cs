using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace WaterGuns.Items.Hardmode
{
    public class MiniWaterGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            DisplayName.SetDefault("Mini Water Gun");
            Tooltip.SetDefault("Rapid but inaccurate\nRight click to place a turret\nDrops from SantaNK1");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 51;
            Item.knockBack = 4;

            Item.useTime = 6;
            Item.useAnimation = 6;
            Item.shoot = ModContent.ProjectileType<Projectiles.Hardmode.MiniWaterProjectile>();
            base.defaultInaccuracy = 8;

            base.offsetAmount = new Vector2(6, 6);
            base.offsetIndependent = new Vector2(0, 14);
        }

        public override bool AltFunctionUse(Player player)
        {
            if (player.HasBuff<Buffs.TurretSummonBuff>())
            {
                turret.Center = player.Center;
            }
            else
            {
                player.AddBuff(ModContent.BuffType<Buffs.TurretSummonBuff>(), 60);
                turret = Projectile.NewProjectileDirect(Projectile.GetSource_NaturalSpawn(), player.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.Hardmode.TurretWaterProjectile>(), 0, 0, player.whoAmI);
            }

            return base.AltFunctionUse(player);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-14, 14);
        }

        Projectile turret = null;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var proj = base.SpawnProjectile(player, source, position, velocity, type, damage, knockback);

            player.itemRotation = proj.velocity.ToRotation() - (player.direction == -1 ? MathHelper.Pi : 0);

            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddCondition(NetworkText.FromLiteral("Mods.WaterGuns.Conditions.Never"), (_) => false);
            recipe.Register();
        }
    }
}
