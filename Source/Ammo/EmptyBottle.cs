using AquaRegia.Library;
using AquaRegia.Library.Extended.Modules;
using Terraria;
using Terraria.ID;

namespace AquaRegia.Ammo;

public class EmptyBottle : BaseItem
{
    public override string Texture => Assets.Sprites.Ammo.empty_bottle;

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
        CreateRecipe(25)
            .AddIngredient(ItemID.Glass, 1)
            .AddTile(TileID.Furnaces)
            .Register();
    }
}