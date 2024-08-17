using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using AquaRegia.Utils;

namespace AquaRegia.Armor.Wooden;

[AutoloadEquip(EquipType.Head)]
public class WoodenHead : ModItem
{
    public static readonly int RangePercent = 3;
    public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(RangePercent);
    public override string Texture => TexturesPath.Armor + "Wooden/WoodenHead";

    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 20;
        Item.value = Item.sellPrice(silver: 1);
        Item.rare = ItemRarityID.White;
        Item.defense = 1;
    }

    public override void UpdateEquip(Player player)
    {
        base.UpdateEquip(player);

        player.GetDamage(DamageClass.Ranged) += RangePercent / 100f;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(ItemID.Wood, 10);
        recipe.AddIngredient(ItemID.Rope, 5);
        recipe.AddTile(TileID.WorkBenches);
        recipe.Register();
    }
}
