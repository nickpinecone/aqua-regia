using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WaterGuns.Utils;

namespace WaterGuns.Armor.Wooden;

[AutoloadEquip(EquipType.Legs)]
public class WoodenLegs : ModItem
{
    public override string Texture => TexturesPath.Armor + "Wooden/WoodenLegs";

    public override void SetDefaults()
    {
        Item.width = 23;
        Item.height = 23;
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
        recipe.AddIngredient(ItemID.Wood, 20);
        recipe.AddIngredient(ItemID.Rope, 10);
        recipe.AddTile(TileID.WorkBenches);
        recipe.Register();
    }
}
