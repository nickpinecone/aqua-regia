using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Items.PreHardmode
{
    public class LavaWaterGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lavashark");
            Tooltip.SetDefault("Sets your enemies ablaze\nFull Pump: Enters fire breathing mode for a few seconds");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            base.isOffset = true;
            base.defaultInaccuracy = 3f;
            base.offsetAmount = new Vector2(6, 6);

            Item.damage = 31;
            Item.knockBack = 5;
            Item.shoot = ModContent.ProjectileType<Projectiles.PreHardmode.LavaWaterProjectile>();
            Item.useTime -= 8;
            Item.useAnimation -= 8;
        }

        bool fireBreath = false;
        int count = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (fireBreath)
            {
                damage *= 2;
                base.SpawnProjectile(player, source, position, velocity, ModContent.ProjectileType<Projectiles.PreHardmode.FireBreath>(), damage, knockback);
                count += 1;
                if (count >= 30)
                {
                    count = 0;
                    fireBreath = false;

                    Item.useTime += 8;
                    Item.useAnimation += 8;
                }
                return false;
            }
            else if (pumpLevel >= 10)
            {
                Item.useTime -= 8;
                Item.useAnimation -= 8;
                pumpLevel = 0;
                fireBreath = true;
                return false;
            }
            else
            {
                return base.Shoot(player, source, position, velocity, type, damage, knockback);
            }
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-34, -2);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.HellstoneBar, 16);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
