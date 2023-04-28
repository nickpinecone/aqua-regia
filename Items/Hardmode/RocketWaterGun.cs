using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace WaterGuns.Items.Hardmode
{
    public class RocketWaterGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            DisplayName.SetDefault("Rocket Water Launcher");
            Tooltip.SetDefault("Shoots rockets that explode into water projctiles");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 73;
            Item.knockBack = 5;
            Item.shoot = ModContent.ProjectileType<Projectiles.Hardmode.WaterProjectile>();
            Item.useTime -= 8;
            Item.useAnimation -= 8;

            base.offsetAmount = new Vector2(6, 6);
            base.offsetIndependent = new Vector2(0, -8);
        }

        int shot = 0;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            base.defaultInaccuracy = 2;
            shot += 1;
            if (shot >= 4)
            {
                SoundEngine.PlaySound(SoundID.Item11);

                for (int i = -1; i < 2; i += 2)
                {
                    var offset = position + velocity.RotatedBy(MathHelper.ToDegrees(90 * i)) * 1.1f;
                    SpawnProjectile(player, source, offset, velocity, ModContent.ProjectileType<Projectiles.Hardmode.RocketWaterProjectile>(), damage / 2, knockback);
                }
                shot = 0;
            }

            base.defaultInaccuracy = 2;
            var proj = base.SpawnProjectile(player, source, position, velocity, type, damage, knockback);
            proj.timeLeft += 20;
            proj.penetrate = 2;

            return false;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-20, -6);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.FragmentVortex, 18);
            recipe.AddTile(412); // Ancient manipulator
            recipe.Register();
        }
    }
}
