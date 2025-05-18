using Terraria;
using Terraria.ModLoader;
using WaterGuns.Utils;
using Terraria.ID;
using WaterGuns.Modules.Ammo;

namespace WaterGuns.Ammo;

public class BottledWater : BaseAmmo
{
    public override string Texture => TexturesPath.Ammo + "BottledWater";

    public override void SetDefaults()
    {
        base.SetDefaults();

        SetProperties(1, 0.1f, ItemRarityID.White, Item.sellPrice(0, 0, 0, 5));
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe(25);
        recipe.AddIngredient(ModContent.ItemType<EmptyBottle>(), 25);
        recipe.AddCondition(Condition.NearWater);
        recipe.Register();
    }
}
