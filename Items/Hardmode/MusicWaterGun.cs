using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Items.Hardmode
{
    public class MusicWaterGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault(
                "Releases an arc of music every third shot" +
                "'Playes a sick beat'"
            );
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            base.isOffset = false;

            Item.damage = 55;
            Item.knockBack = 5;
            Item.shoot = ModContent.ProjectileType<Projectiles.Hardmode.TitaniumWaterProjectile>();
        }

        int shot = 0;
        int[] notes = { ProjectileID.EighthNote, ProjectileID.QuarterNote, ProjectileID.TiedEighthNote };

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            shot += 1;
            if (shot >= 5)
            {
                for (int i = -5; i < 5; i++)
                {
                    int noteNum = Main.rand.Next(0, 3);
                    var modifiedVelocity = velocity.RotatedBy(MathHelper.ToRadians(i * 5)) / 2f;
                    Projectile.NewProjectile(Projectile.GetSource_NaturalSpawn(), position, modifiedVelocity, notes[noteNum], damage, knockback, player.whoAmI);

                }
                shot = 0;
            }
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.PixieDust, 18);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
