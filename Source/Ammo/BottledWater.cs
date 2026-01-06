using AquaRegia.Library;
using AquaRegia.Library.Extended.Modules;
using AquaRegia.Library.Extended.Modules.Items;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace AquaRegia.Ammo;

public class BottledWater : BaseAmmo
{
    public override string Texture => Assets.Sprites.Ammo.bottled_water;

    private PropertyModule Property { get; } = new();

    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 50;
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.Set(this)
            .Defaults.BottledWater()
            .Damage(1, 0.1f)
            .Rarity(ItemRarityID.White)
            .Price(Item.sellPrice(copper: 5));
    }

    public override void AddRecipes()
    {
        CreateRecipe(25)
            .AddIngredient(ModContent.ItemType<EmptyBottle>(), 25)
            .AddCondition(Condition.NearWater)
            .Register();
    }
}