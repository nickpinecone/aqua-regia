using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Items.PreHardmode
{
    public class DemonWaterGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            DisplayName.SetDefault("Demonic Flow");
            Tooltip.SetDefault("Spawns an additional stream of water upon impact\nFull Pump: Spawns two water swords at your cursor");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 14;
            Item.knockBack = 4;
            Item.shoot = ModContent.ProjectileType<Projectiles.PreHardmode.DemonWaterProjectile>();

            Item.useAnimation += 4;
            Item.useTime += 4;
            Item.shootSpeed += 2;
            base.defaultInaccuracy = 2f;
            base.maxPumpLevel = 8;


            base.offsetIndependent = new Vector2(0, -3);
            base.offsetAmount = new Vector2(4, 4);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (pumpLevel >= maxPumpLevel)
            {
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item8);

                int rotation = Main.rand.Next(0, 360);
                var randomPosition = Main.MouseWorld + new Vector2(196 + Main.rand.Next(-32, 32), 0).RotatedBy(MathHelper.ToRadians(-45)).RotatedBy(MathHelper.ToRadians(rotation));
                var modifiedVelocity = new Vector2(10, 0).RotatedBy(MathHelper.ToRadians(rotation - 180 - 45));

                var proj = Projectile.NewProjectileDirect(source, randomPosition, modifiedVelocity, ModContent.ProjectileType<Projectiles.PreHardmode.SwordSlash>(), damage * 3, knockback, player.whoAmI);
                proj.rotation = MathHelper.ToRadians(rotation - 180);
                proj.scale = 2;

                rotation = rotation - 180 + Main.rand.Next(-45, 45);
                randomPosition = Main.MouseWorld + new Vector2(196 + Main.rand.Next(-32, 32), 0).RotatedBy(MathHelper.ToRadians(-45)).RotatedBy(MathHelper.ToRadians(rotation));
                modifiedVelocity = new Vector2(10, 0).RotatedBy(MathHelper.ToRadians(rotation - 180 - 45));

                var proj2 = Projectile.NewProjectileDirect(source, randomPosition, modifiedVelocity, ModContent.ProjectileType<Projectiles.PreHardmode.SwordSlash>(), damage * 3, knockback, player.whoAmI);
                proj2.rotation = MathHelper.ToRadians(rotation - 180);
                proj2.scale = 2;
            }

            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-12, 0);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DemoniteBar, 18);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
