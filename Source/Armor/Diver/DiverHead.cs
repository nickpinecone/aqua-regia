using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using WaterGuns.Utils;

namespace WaterGuns.Armor.Diver;

[AutoloadEquip(EquipType.Head)]
public class DiverHead : ModItem
{
    public static readonly int RangePercent = 4;
    public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(RangePercent);
    public override string Texture => TexturesPath.Armor + "Diver/DiverHead";

    public override void SetDefaults()
    {
        Item.width = 22;
        Item.height = 20;
        Item.value = Item.sellPrice(gold: 1);
        Item.rare = ItemRarityID.Blue;
        Item.defense = 4;
    }

    public override void UpdateEquip(Player player)
    {
        base.UpdateEquip(player);

        player.GetDamage(DamageClass.Ranged) += RangePercent / 100f;
        player.gills = true;
    }

    public override void AddRecipes()
    {
        Recipe recipe1 = CreateRecipe();
        recipe1.AddIngredient(ItemID.DemoniteBar, 10);
        recipe1.AddIngredient(ItemID.ShadowScale, 5);
        recipe1.AddIngredient(ItemID.Starfish, 3);
        recipe1.AddTile(TileID.WorkBenches);
        recipe1.Register();

        Recipe recipe2 = CreateRecipe();
        recipe2.AddIngredient(ItemID.CrimtaneBar, 10);
        recipe2.AddIngredient(ItemID.TissueSample, 5);
        recipe2.AddIngredient(ItemID.Starfish, 3);
        recipe2.AddTile(TileID.WorkBenches);
        recipe2.Register();
    }
}
