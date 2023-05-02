using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace WaterGuns.Items.Hardmode
{
    public class OceanWaterGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            DisplayName.SetDefault("Ocean Overlord");
            Tooltip.SetDefault("Every third shot releases a waternado that chases enemies\nDrops from Duke Fishron");
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-20, -4);
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 59;
            Item.knockBack = 5;
            Item.shoot = ModContent.ProjectileType<Projectiles.Hardmode.OceanWaterProjectile>();
        }

        int shot = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = -1; i < 2; i += 2)
            {
                int distanceBetween = 1;
                Vector2 modifiedVelocity = velocity.RotatedBy(MathHelper.ToRadians(distanceBetween * i * player.direction));
                base.SpawnProjectile(player, source, position, modifiedVelocity, type, damage, knockback);
            }

            shot += 1;
            if (shot >= 3)
            {
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item21);
                shot = 0;
                position = new Vector2(position.X + velocity.X * 3, position.Y + velocity.Y * 3);
                Projectile.NewProjectile(source, position, velocity * 1.4f, ModContent.ProjectileType<Projectiles.Hardmode.Waternado>(), damage, knockback, player.whoAmI);
            }


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
