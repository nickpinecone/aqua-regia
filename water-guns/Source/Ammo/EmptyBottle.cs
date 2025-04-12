using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WaterGuns.Utils;

namespace WaterGuns.Ammo;

public class EmptyBottle : ModItem
{
    public override string Texture => TexturesPath.Ammo + "EmptyBottle";

    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 25;
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Item.maxStack = Item.CommonMaxStack;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe(25);
        recipe.AddIngredient(ItemID.Glass, 1);
        recipe.AddTile(TileID.Furnaces);
        recipe.Register();
    }
}
