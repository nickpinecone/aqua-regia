using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace WaterGuns.Items.PreHardmode
{
    public class ChainedWaterGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Spawns an additional stream of water upon impact");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ChainGuillotines);

            Item.DamageType = DamageClass.Ranged;
            Item.autoReuse = true;
            Item.damage = 25;
            Item.knockBack = 4;
            Item.shoot = ModContent.ProjectileType<Projectiles.PreHardmode.ChainedWaterProjectile>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Chain, 3);
            recipe.AddIngredient(ItemID.IronBar, 20);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
