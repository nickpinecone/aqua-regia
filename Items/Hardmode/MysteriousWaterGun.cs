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
            DisplayName.SetDefault("Mysterious Hydropump");
            Tooltip.SetDefault("\"Power, Unlimited Power!\"");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            base.defaultInaccuracy = 0;

            Item.damage = 92;
            Item.knockBack = 6;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 3; i++)
            {
                var proj = base.SpawnProjectile(player, source, position, velocity, ModContent.ProjectileType<WaterProjectile>(), damage / 2, knockback);
                proj.tileCollide = false;
                proj.penetrate = -1;
                proj.timeLeft = 120;
                if (i == 1)
                    proj.timeLeft = 85;
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
