using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace WaterGuns.Armors.PreHardmode.WoodenArmor
{
    public class WoodenChestplate : BaseArmors.BaseChestplate
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Wooden Bikini");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 20;
            Item.height = 20;
            Item.defense = 2;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == ModContent.ItemType<WoodenHelmet>() && legs.type == ModContent.ItemType<WoodenPants>();
        }

        public override void UpdateArmorSet(Player player)
        {
            base.UpdateArmorSet(player);
        }

        public override void UpdateEquip(Player player)
        {
            base.UpdateEquip(player);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Wood, 20);
            recipe.AddIngredient(ItemID.Acorn, 10);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
