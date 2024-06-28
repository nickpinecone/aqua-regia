using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Ammo;

public class EmptyBottle : ModItem
{
    public override string Texture => "WaterGuns/Assets/Textures/Ammo/EmptyBottle";

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
