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
    public abstract class MysteriousWaterGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            DisplayName.SetDefault("-WIP- Mysterious Hydropump");
            Tooltip.SetDefault("\"Power, Unlimited Power!\"");
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-16, 0);
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            base.defaultInaccuracy = 0;
            base.offsetAmount = new Vector2(6, 6);
            base.offsetIndependent = new Vector2(0, -6);

            Item.damage = 92;
            Item.knockBack = 6;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 3; i++)
            {
                var proj = base.SpawnProjectile(player, source, position, velocity * 1.3f, ModContent.ProjectileType<WaterProjectile>(), damage / 2, knockback);
                proj.tileCollide = false;
                proj.penetrate = -1;
                proj.timeLeft = 110;
                if (i == 1)
                    proj.timeLeft = 75;
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
