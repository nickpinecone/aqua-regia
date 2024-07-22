using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WaterGuns.Utils;

namespace WaterGuns.Armor.Wooden;

[AutoloadEquip(EquipType.Head)]
public class WoodenHead : ModItem
{
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

        player.GetDamage(DamageClass.Ranged) += 0.03f;
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
