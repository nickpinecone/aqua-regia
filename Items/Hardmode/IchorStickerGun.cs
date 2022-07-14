using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Items.Hardmode
{
    public class IchorStickerGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ichor Sticker Blaster");
            Tooltip.SetDefault("Inflicts ichor debuff");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 35;
            Item.knockBack = 3;
            Item.shoot = ModContent.ProjectileType<Projectiles.Hardmode.IchorStickerProjectile>();
            Item.shootSpeed -= 4;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-8, 0);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Ichor, 18);
            recipe.AddIngredient(ModContent.ItemType<PreHardmode.CrimsonWaterGun>(), 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
