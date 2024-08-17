using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using AquaRegia.Modules;
using AquaRegia.Utils;

namespace AquaRegia.Ammo;

public class BottledWater : BaseAmmo
{
    public override string Texture => TexturesPath.Ammo + "BottledWater";

    public override void SetDefaults()
    {
        base.SetDefaults();

        Item.damage = 1;
        Item.knockBack = 0.1f;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe(25);
        recipe.AddIngredient(ModContent.ItemType<EmptyBottle>(), 25);
        recipe.AddCondition(Condition.NearWater);
        recipe.Register();
    }
}
